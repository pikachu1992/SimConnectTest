using Microsoft.FlightSimulator.SimConnect;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SimLib
{
    public enum REQUESTS { }

    public enum DEFINITIONS { }

    public static class SimObjectType<T>
    {
        public struct Field
        {
            public string DatumName
            { get; set; }

            public string UnitsName
            { get; set; }

            public SIMCONNECT_DATATYPE DatumType
            { get; set; }
        }

        private static Dictionary<int, TaskCompletionSource<T>> tasks =
            new Dictionary<int, TaskCompletionSource<T>>();

        public static async Task<T> GetAsync(
            uint radius = 0,
            SIMCONNECT_SIMOBJECT_TYPE type = SIMCONNECT_SIMOBJECT_TYPE.USER)
        {
            TaskCompletionSource<T> task = new TaskCompletionSource<T>();

            tasks.Add(task.Task.Id, task);
            FSX.Sim.
                RequestDataOnSimObjectType(
                (REQUESTS)task.Task.Id,
                (DEFINITIONS)FSX.typeMap[typeof(T)],
                radius,
                type);
            
            T result = await task.Task;

            tasks.Remove(task.Task.Id);
            return result;
        }

        public static void Register(Field[] fields)
        {
            int defineId = FSX.idMap.Count;

            try
            {
                // register all fields in FSX.Sim
                // TODO: lookup type decorated fields using reflection
                foreach (Field field in fields)
                    FSX.Sim.
                        AddToDataDefinition((DEFINITIONS)defineId,
                                            field.DatumName,
                                            field.UnitsName,
                                            field.DatumType,
                                            0.0f,
                                            SimConnect.SIMCONNECT_UNUSED);

                FSX.Sim.
                    RegisterDataDefineStruct<T>((DEFINITIONS)defineId);

                FSX.Sim.OnRecvSimobjectDataBytype +=
                    Sim_OnRecvSimobjectDataBytype;
            }
            catch (COMException ex)
            {
                throw ex;
            }

            FSX.idMap.Add(defineId, typeof(T));
            FSX.typeMap.Add(typeof(T), defineId);
        }

        private static void Sim_OnRecvSimobjectDataBytype(
            SimConnect sender,
            SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            TaskCompletionSource<T> task;
            tasks.TryGetValue((int)data.dwRequestID, out task);

            if (task != null)
                task.TrySetResult((T)data.dwData[0]);
        }
    }
}

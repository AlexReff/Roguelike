using ReGoap.Core;

namespace ReGoap
{
    internal class ReGoapMemoryAdvanced<T, W> : ReGoapMemory<T, W>
    {
        private IReGoapSensor<T, W>[] sensors;

        public float SensorsUpdateDelay = 0.3f;
        private float sensorsUpdateCooldown;

        public ReGoapMemoryAdvanced() : base()
        {
            //sensors = GetComponents<IReGoapSensor<T, W>>();
            //foreach (var sensor in sensors)
            //{
            //    sensor.Init(this);
            //}
        }

        protected virtual void Update()
        {
            //if (Time.time > sensorsUpdateCooldown)
            //{
            //    sensorsUpdateCooldown = Time.time + SensorsUpdateDelay;

            //    foreach (var sensor in sensors)
            //    {
            //        sensor.UpdateSensor();
            //    }
            //}
        }

        //#region UnityFunctions
        //protected override void Awake()
        //{
        //    base.Awake();
        //    sensors = GetComponents<IReGoapSensor<T, W>>();
        //    foreach (var sensor in sensors)
        //    {
        //        sensor.Init(this);
        //    }
        //}

        //protected virtual void Update()
        //{
        //    if (Time.time > sensorsUpdateCooldown)
        //    {
        //        sensorsUpdateCooldown = Time.time + SensorsUpdateDelay;

        //        foreach (var sensor in sensors)
        //        {
        //            sensor.UpdateSensor();
        //        }
        //    }
        //}
        //#endregion
    }
}

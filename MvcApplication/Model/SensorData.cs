using System;

namespace MvcApplication.Model
{
    public class SensorData
    {
        public SensorData() { }

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public double Distance { get; set; }

        public string UserId { get; set; }

    }
}
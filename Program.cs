using System.Collections;

namespace UMICH_Problem_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IschemiaPredictor test = new IschemiaPredictor();
            List<EKG> ekgList = new List<EKG>();

            ekgList.Add(new EKG(DateTime.Now, 0.1));
            ekgList.Add(new EKG(DateTime.Now, 0.8));
            ekgList.Add(new EKG(DateTime.Now, 1.5));
            ekgList.Add(new EKG(DateTime.Now, 1.8));
            ekgList.Add(new EKG(DateTime.Now, 2.1));

            foreach (var ekg in ekgList)
            {
                test.ProcessEKG(ekg);
            }
        }
    }

    // EKG Class that contains the time and the value of the ST Segement
    public class EKG
    {
        public DateTime Time { get; set; }
        public double STvalue { get; set; }

        public EKG(DateTime Time, double STvalue) 
        {
            this.Time = Time;
            this.STvalue = STvalue;
        }
    }

    public class IschemiaPredictor
    {
        // Queue to keep track of the measurements. First in first out.
        private Queue<EKG> ekg_Measurements= new Queue<EKG>();

        // Fields to keep track of minimum and maximum
        private double minValue = double.MaxValue;
        private double maxValue = double.MinValue;

        // Function that passes EKG measurement as an argument and adds it to the queue.
        public void ProcessEKG(EKG measurement)
        {
            // Adds the EKG measurement to the queue
            ekg_Measurements.Enqueue(measurement);

            // Sets the minValue field to the minimum value in the queue
            // Sets the maxValue field to the maximum value to the queue
            minValue = Math.Min(minValue, measurement.STvalue);
            maxValue = Math.Max(maxValue, measurement.STvalue);

            // While loop that continuously checks if there are more than 60 measurements (2 minutes worth at 2 second intervals)
            // If more than 60 measurements, remove the oldest measurement, and then reset min/max values
            while (ekg_Measurements.Count > 60) 
            {
                EKG oldestMeasurement = ekg_Measurements.Dequeue();

                if (oldestMeasurement.STvalue == minValue || oldestMeasurement.STvalue == maxValue)
                {
                    minValue = double.MaxValue;
                    maxValue = double.MinValue;
                    foreach (EKG ekgMeasurement in ekg_Measurements)
                    {
                        minValue = Math.Min(minValue, ekgMeasurement.STvalue);
                        maxValue = Math.Max(maxValue, ekgMeasurement.STvalue);
                    }
                }
            }

            // Conditional statement to check if the min or max ST values surpass their thresholds
            if (minValue < -2 || maxValue > 2 || (maxValue - minValue) > 2)
            {
                Alarm();
            }

        }

        // Alarm function that is used if min value or max value surpass thresholds
        private void Alarm()
        {
            Console.WriteLine($"There is an indication of ischemia! The patients minimum value is {minValue} and their maximum value is {maxValue}");
        }
    }
}
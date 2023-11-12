using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace model_metro
{
    internal class TrafficPassengercs
    {
        Random random = new Random();
        Dictionary<int, int> passengerFlow = new Dictionary<int, int>
        {
            {6,1600},
            {7, 1800 },
            {8, 2000 },
            {9, 1400 },
            {10,800 },
            {11,800 },
            {12,700 },
            {13,600 },
            {14,800 },
            {15,800 },
            {16,800 },
            {17,1000 },
            {18,2000 },
            {19,1800 },
            {20,500 },
            {21,200 },
            {22,200 },
            {23,200 },
            {24,20 },
        };

        public List<Passenger> GeneratePassengers(int startTime, int endTime)
        {
            List<Passenger> passengers = new List<Passenger>();
            
            for (int hour = startTime; hour <= endTime+1; hour++)
            {
                double randomValue = random.NextDouble() * 0.2 + 0.9;
                for (int i = 0; i < randomValue * passengerFlow[startTime]; i++)
                {
                    int startStation, endStation, transferStation;
                    bool isGreenLine = random.Next(2) == 1;
                    if (isGreenLine)
                    {
                        startStation = random.Next(1, 6);
                        do
                        {
                            endStation = random.Next(1, 6);
                        } while (endStation == startStation);
                        transferStation = 2;

                        if (endStation == transferStation && endStation != startStation)
                            do { endStation = random.Next(1, 9); } while (endStation == transferStation || endStation == 3);
                        else
                        {
                            transferStation = 0;
                            do { startStation = random.Next(1, 6); } while (startStation == endStation);
                        }

                    }
                    else
                    {
                        startStation = random.Next(1, 9);
                        transferStation = 5;
                        do
                        {
                            endStation = random.Next(1, 9);
                        } while (endStation == startStation);
                        if (endStation == transferStation && endStation != startStation)
                            do { endStation = random.Next(1, 6); } while (endStation == transferStation || endStation==2);
                        else
                        {
                            transferStation = 0;
                            do { startStation = random.Next(1, 9); } while (startStation == endStation);
                        }
                    }
                    double passengerCount = RandomHelper.NextGaussian(passengerFlow[hour], passengerFlow[hour] / 5);
                    int arrivalTime = hour * 60 + (int)passengerCount % 60;


                    passengers.Add(new Passenger
                    {
                        GreenLine = isGreenLine,
                        StartStation = startStation,
                        EndStation = endStation,
                        TransferStation = transferStation,
                        ArrivalTime = arrivalTime,
                    });
                }
            }
            return passengers;
        }


    }
}

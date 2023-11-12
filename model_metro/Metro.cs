using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace model_metro
{
    internal class Metro
    {
        List<Passenger> pass;
        private int timeRline;
        private int timeGline;
        private int endTime;
        Random rand = new Random();
        enum RedLineStations
        {
            Zaeltsovskaya = 1,
            Gagarinskaya,
            KrasnyProspect,
            PloshchadLenina,
            Oktyabrskaya,
            RechnoyVokzal,
            Studencheskaya,
            PloshchadMarksa
        }
        enum GreenLineStations
        {
            PloshchadGarinaMikhailovskogo = 1,
            Sibirskay,
            MarshalaPokryshkina,
            BeryozovayaRoscha,
            ZolotayaNiva
        }
        public Metro(int t_Rline, int t_Gline, int startTime, int endTime)
        {
            TrafficPassengercs traffic = new TrafficPassengercs();
            pass = traffic.GeneratePassengers(startTime/60, endTime/60);
            timeRline = t_Rline;
            timeGline = t_Gline;
            this.endTime = endTime;
        }

        public void Simulate()
        {
            pass.Sort((x, y) => x.ArrivalTime.CompareTo(y.ArrivalTime));
            foreach (Passenger passenger in pass)
            {
                if (passenger.ArrivalTime > endTime) return;
                if ((passenger.GreenLine) && (passenger.TransferStation == 0))
                {
                    passenger.Delay = timeGline - passenger.ArrivalTime % timeGline;
                    passenger.DepartureTime = passenger.ArrivalTime + Math.Abs(passenger.StartStation - passenger.EndStation) * timeGline + passenger.Delay;
                }
                else if (passenger.TransferStation == 0)
                {
                    passenger.Delay = timeRline - passenger.ArrivalTime % timeRline;
                    passenger.DepartureTime = passenger.ArrivalTime + Math.Abs(passenger.StartStation - passenger.EndStation) * timeRline + passenger.Delay;

                }
                else if (passenger.GreenLine)
                {
                    passenger.Delay = (timeGline - passenger.ArrivalTime % timeGline) + (timeRline - (rand.Next(1, 3) + passenger.ArrivalTime) % timeRline);
                    passenger.DepartureTime = passenger.Delay + passenger.ArrivalTime + Math.Abs(passenger.StartStation - passenger.TransferStation) * timeGline +
                        Math.Abs(passenger.EndStation - passenger.TransferStation) * timeRline;
                }
                else
                {
                    passenger.Delay = (timeRline - passenger.ArrivalTime % timeRline) + (timeGline - (rand.Next(1, 3) + passenger.ArrivalTime) % timeGline);
                    passenger.DepartureTime = passenger.Delay + passenger.ArrivalTime + Math.Abs(passenger.EndStation - passenger.TransferStation) * timeGline +
                        Math.Abs(passenger.StartStation - passenger.TransferStation) * timeRline;
                }
            }
        }
        private string pathPassenger(Passenger p)
        {
            string path = "";
            if (p.GreenLine && p.TransferStation == 0)
                path = Enum.GetName(typeof(GreenLineStations), p.StartStation) + "-" + Enum.GetName(typeof(GreenLineStations), p.EndStation);
            else if (!p.GreenLine && p.TransferStation == 0)
                path = Enum.GetName(typeof(RedLineStations), p.StartStation) + "-" + Enum.GetName(typeof(RedLineStations), p.EndStation);
            else if (p.TransferStation == 2)
                path = Enum.GetName(typeof(GreenLineStations), p.StartStation) + "-" + Enum.GetName(typeof(GreenLineStations), 2) +
                "-" + Enum.GetName(typeof(RedLineStations), 3) + "-" + Enum.GetName(typeof(RedLineStations), p.EndStation);
            else if (p.TransferStation == 5)
                path = Enum.GetName(typeof(RedLineStations), p.StartStation) + "-" + Enum.GetName(typeof(RedLineStations), 3) +
                "-" + Enum.GetName(typeof(GreenLineStations), 2) + "-" + Enum.GetName(typeof(GreenLineStations), p.EndStation);
            return path;

        }
        private string FormatTime(int t)
        {
            int hours = t / 60;
            int minutes = t % 60;

            // Добавляем ведущие нули для однозначных часов и минут
            string formattedHours = hours.ToString("D2");
            string formattedMinutes = minutes.ToString("D2");

            return $"{formattedHours}:{formattedMinutes}";
        }
        public void ExportToExcel(string filename)
        {
            pass.Sort((x, y) => x.ArrivalTime.CompareTo(y.ArrivalTime));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(filename);

                worksheet.Cells[1, 1].Value = "Путь";
                worksheet.Cells[1, 2].Value = "Время отправления";
                worksheet.Cells[1, 3].Value = "Время в пути";
                worksheet.Cells[1, 4].Value = "Время прибытия";

                int row = 2;

                foreach (var passenger in pass)
                {
                    if (passenger.DepartureTime == 0) break;
                    string path = pathPassenger(passenger);

                    int departureTime = passenger.ArrivalTime + passenger.Delay;

                    int travelTime = passenger.DepartureTime - passenger.ArrivalTime - passenger.Delay;

                    int arrivalTime = passenger.DepartureTime;

                    worksheet.Cells[row, 1].Value = path;
                    worksheet.Cells[row, 2].Value = FormatTime(departureTime);
                    worksheet.Cells[row, 3].Value = FormatTime(travelTime);
                    worksheet.Cells[row, 4].Value = FormatTime(arrivalTime);

                    row++;
                }

                var fileInfo = new FileInfo(filename+".xlsx");
                package.SaveAs(fileInfo);
            }
        }
    }

}


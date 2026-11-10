using System.Collections.Generic;

public class IrctcTicket
{
    public string pnrNumber { get; set; }

    public string transactionID { get; set; }

    public string ticketID { get; set; }

    public string trainNo { get; set; }

    public string trainName { get; set; }

    public string classCode { get; set; }

    public string quota { get; set; }

    public string dateOfJourney { get; set; }

    public string dateOfBooking { get; set; }

    public string fromStation { get; set; }

    public string toStation { get; set; }

    public string fromStationName { get; set; }

    public string toStationName { get; set; }

    public string boardingAt { get; set; }

    public string boardingAtName { get; set; }

    public string reservationUpto { get; set; }

    public string reservationUptoName { get; set; }

    public string distance { get; set; }

    public string scheduledDepartureTime { get; set; }

    public string scheduledArrivalTime { get; set; }

    public string qrCodeText { get; set; }

    public List<Passenger> passengerList { get; set; }

    public List<FareInfo> fareInfoData { get; set; }

    public List<FareInfo> fareDescAll { get; set; }

    public List<string> bottomTicketMessage { get; set; }
}

public class Passenger
{
    public string passengerSerialNumber { get; set; }

    public string passengerName { get; set; }

    public string passengerAge { get; set; }

    public string passengerGender { get; set; }

    public string bookingStatus { get; set; }

    public string currentStatus { get; set; }

    public string bookingBerthCode { get; set; }

    public string bookingBerthNo { get; set; }

    public string currentBerthCode { get; set; }

    public string currentBerthNo { get; set; }
}

public class FareInfo
{
    public string paramName { get; set; }

    public string message { get; set; }
}
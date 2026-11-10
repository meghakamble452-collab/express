using System.Collections.Generic;

public class Root
{
    public List<BookingResponse> bookingResponseList { get; set; }
    public string serverId { get; set; }
    public string timeStamp { get; set; }
    public string eligibleForCancel { get; set; }
    public string showLastTxn { get; set; }
}

public class BookingResponse
{
    public string trainName { get; set; }
    public string distance { get; set; }
    public string boardingStn { get; set; }
    public string boardingDate { get; set; }
    public string journeyDate { get; set; }
    public string transactionId { get; set; }
    public string reservationId { get; set; }
    public string pnrNumber { get; set; }
    public string departureTime { get; set; }
    public string arrivalTime { get; set; }
    public string bookingDate { get; set; }
    public string trainNumber { get; set; }
    public string fromStn { get; set; }
    public string destStn { get; set; }
    public string journeyClass { get; set; }
    public string journeyQuota { get; set; }
    public string totalFare { get; set; }
    public string totalCollectibleAmount { get; set; }
    public string ticketType { get; set; }
    public string reservationStatus { get; set; }
    public string trainChartStatus { get; set; }
    public string insuranceCompany { get; set; }
    public string insuranceOpted { get; set; }

    public List<Passenger123> psgnDtlList { get; set; }
    public List<ReservationDetail> resvDetails { get; set; }
}

public class Passenger123
{
    public string passengerSerialNumber { get; set; }
    public string passengerName { get; set; }
    public string passengerAge { get; set; }
    public string passengerGender { get; set; }
    public string bookingStatus { get; set; }
    public string bookingCoachId { get; set; }
    public string bookingBerthNo { get; set; }
    public string bookingBerthCode { get; set; }
    public string bookingStatusDetails { get; set; }
    public string currentStatus { get; set; }
    public string currentCoachId { get; set; }
    public string currentBerthNo { get; set; }
    public string currentStatusDetails { get; set; }
    public string passengerNetFare { get; set; }
}

public class ReservationDetail
{
    public string reservationId { get; set; }
    public string reservationMode { get; set; }
    public string transactionDate { get; set; }
    public string noOfTicket { get; set; }
    public string totalAmntRes { get; set; }
    public string paymentType { get; set; }
    public string refundStatusRes { get; set; }
    public string userId { get; set; }
    public string mobileNumber { get; set; }
    public string reservationStatus { get; set; }
}
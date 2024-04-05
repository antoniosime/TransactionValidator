The basis of this test is to collaborate the design and implementation of an Event-Driven service to monitor transactions for a financial system, the micro service should be capable of processing transaction events and raising alerts for potentially fraudulent activities, this will involve observability, velocity limits and thresholds and sanctions.

Below are list of requirements of the service, the format of this test will be as such:
-	Design of the system 
-	Implement the durable function in C#

Velocity Limits - The amount that can move in and out of an account in a day 
Payment Thresholds - The maximum amount per transaction allowed 
Sanctions - Restrictions on transactions given source or destination countries

Current Tenant settings
```json
{
    "tenantsettings": [
        {
            "tenantid": "345",
            "velocitylimits": {
                "daily": "2500"
            },
            "thresholds": {
                "pertransaction": "1500"
            },
            "countrysanctions": {
                "sourcecountrycode": "AFG, BLR, BIH, IRQ, KEN, RUS",
                "destinationcountrycode": "AFG, BLR, BIH, IRQ, KEN, RUS, TKM, UGA"
            }
        }
    ]
}
```

Incoming topic message contains the following JSON structure:

```json
{
    "correlationId": "0EC1D320-3FDD-43A0-84B8-3CF8972CDCD8",
    "tenantId": "345",
    "transactionId": "eyJpZCI6ImE2NDUzYTZlLTk1NjYtNDFmOC05ZjAzLTg3ZDVmMWQ3YTgxNSIsImlzIjoiU3RhcmxpbmciLCJydCI6InBheW1lbnQifQ",
    "transactionDate": "2024-02-15 11:36:22",
    "direction": "Credit",
    "amount": "345.87",
    "currency": "EUR",
    "description": "Mr C A Woods",
    "sourceaccount": {
        "accountno": "44421232",
        "sortcode": "30-23-20",
        "countrycode": "GBR"
    },
    "destinationaccount": {
        "accountno": "87285552",
        "sortcode": "10-33-12",
        "countrycode": "HKG"
    }
}
```
NOTES: 
-	Transaction ID is a transport Id of structure //{"id":a6453a6e-9566-41f8-9f03-87d5f1d7a815","is":"HopeBank","rt":"payment"} 
-	 Direction can be Credit or Debit

Requirements: 
-	The service should received messages from the message bus.
-	The service should load the Tenant specific settings given the TenantId.
-	The service should assess the incoming message against the restrictions defined in the Tenant settings.
-	The service should raise an event should any of the restrictions be violated and send the payment to a holding queue for assessment.
-	The service should send the payment to processing queue if no tenant settings are violated.
-	The service should send a message to the operations topic if any exceptions occur during processing.

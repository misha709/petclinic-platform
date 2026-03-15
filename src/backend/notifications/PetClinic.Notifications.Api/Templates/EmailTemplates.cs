namespace PetClinic.Notifications.Api.Templates;

public static class EmailTemplates
{
    public static string VisitConfirmation(
        string ownerName,
        string petName,
        string vetName,
        DateTime scheduledAt,
        string reason) =>
        $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Visit Confirmation</title>
            <style>
                body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
                .container { max-width: 600px; margin: 40px auto; background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
                .header { background-color: #2e7d32; padding: 32px; text-align: center; }
                .header h1 { color: #ffffff; margin: 0; font-size: 24px; }
                .header p { color: #c8e6c9; margin: 8px 0 0; font-size: 14px; }
                .body { padding: 32px; color: #333333; }
                .body h2 { color: #2e7d32; margin-top: 0; }
                .detail-box { background-color: #f1f8e9; border-left: 4px solid #2e7d32; border-radius: 4px; padding: 16px 20px; margin: 20px 0; }
                .detail-row { margin-bottom: 8px; }
                .detail-label { font-weight: bold; display: inline-block; min-width: 120px; color: #555555; }
                .detail-value { color: #333333; }
                .footer { background-color: #f9f9f9; padding: 20px 32px; text-align: center; color: #888888; font-size: 12px; border-top: 1px solid #eeeeee; }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="header">
                    <h1>PetClinic</h1>
                    <p>Your appointment is confirmed</p>
                </div>
                <div class="body">
                    <h2>Hello, {{ownerName}}!</h2>
                    <p>Your visit has been successfully scheduled. Here are the details:</p>
                    <div class="detail-box">
                        <div class="detail-row">
                            <span class="detail-label">Patient:</span>
                            <span class="detail-value">{{petName}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Veterinarian:</span>
                            <span class="detail-value">{{vetName}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Date &amp; Time:</span>
                            <span class="detail-value">{{scheduledAt:dddd, MMMM d, yyyy}} at {{scheduledAt:h:mm tt}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Reason:</span>
                            <span class="detail-value">{{reason}}</span>
                        </div>
                    </div>
                    <p>Please arrive 10 minutes early. If you need to reschedule or cancel, contact us as soon as possible.</p>
                </div>
                <div class="footer">
                    <p>PetClinic &mdash; Caring for your pets</p>
                    <p>This is an automated message, please do not reply.</p>
                </div>
            </div>
        </body>
        </html>
        """;

    public static string VisitReminder(
        string ownerName,
        string petName,
        string vetName,
        DateTime scheduledAt,
        string reason) =>
        $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Visit Reminder</title>
            <style>
                body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
                .container { max-width: 600px; margin: 40px auto; background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
                .header { background-color: #e65100; padding: 32px; text-align: center; }
                .header h1 { color: #ffffff; margin: 0; font-size: 24px; }
                .header p { color: #ffe0b2; margin: 8px 0 0; font-size: 14px; }
                .body { padding: 32px; color: #333333; }
                .body h2 { color: #e65100; margin-top: 0; }
                .detail-box { background-color: #fff3e0; border-left: 4px solid #e65100; border-radius: 4px; padding: 16px 20px; margin: 20px 0; }
                .detail-row { margin-bottom: 8px; }
                .detail-label { font-weight: bold; display: inline-block; min-width: 120px; color: #555555; }
                .detail-value { color: #333333; }
                .badge { display: inline-block; background-color: #e65100; color: #ffffff; padding: 6px 14px; border-radius: 20px; font-weight: bold; font-size: 14px; margin-bottom: 16px; }
                .footer { background-color: #f9f9f9; padding: 20px 32px; text-align: center; color: #888888; font-size: 12px; border-top: 1px solid #eeeeee; }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="header">
                    <h1>PetClinic</h1>
                    <p>Your appointment is coming up soon</p>
                </div>
                <div class="body">
                    <h2>Reminder, {{ownerName}}!</h2>
                    <span class="badge">1 Hour Away</span>
                    <p>This is a friendly reminder that {{petName}}'s appointment is in approximately <strong>1 hour</strong>. Here are the details:</p>
                    <div class="detail-box">
                        <div class="detail-row">
                            <span class="detail-label">Patient:</span>
                            <span class="detail-value">{{petName}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Veterinarian:</span>
                            <span class="detail-value">{{vetName}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Date &amp; Time:</span>
                            <span class="detail-value">{{scheduledAt:dddd, MMMM d, yyyy}} at {{scheduledAt:h:mm tt}}</span>
                        </div>
                        <div class="detail-row">
                            <span class="detail-label">Reason:</span>
                            <span class="detail-value">{{reason}}</span>
                        </div>
                    </div>
                    <p>Please remember to bring any relevant medical records or previous test results.</p>
                </div>
                <div class="footer">
                    <p>PetClinic &mdash; Caring for your pets</p>
                    <p>This is an automated message, please do not reply.</p>
                </div>
            </div>
        </body>
        </html>
        """;
}

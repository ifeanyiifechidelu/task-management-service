using System;
using Stripe;

namespace Task_Management_Service.Domain
{
    public class PaymentException : AppException
    {
        public string ErrorCode { get; set; }

        public PaymentException(string error, string errorCode = "", int httpStatusCode = 500)
            : base(new[] { error }, "PaymentError", httpStatusCode)
        {
            this.ErrorCode = errorCode;
        }
    }

    public static class PaymentExceptionHandler
    {
        public static AppException HandleException(Exception exception)
        {
            if (exception is StripeException stripeException)
            {
                return HandleStripeException(stripeException);
            }
            return new PaymentException(exception.Message, "UnknownError");
        }

      private static AppException HandleStripeException(StripeException exception)
        {
            var stripeError = exception.StripeError;
            string errorCode = stripeError?.Code ?? "UnknownError";

            return errorCode switch
            {
                "card_declined" => new BadRequestException("Your card was declined. Please use a different payment method."),
                "incorrect_number" => new BadRequestException("The card number is incorrect. Please double-check your card details and try again."),
                "invalid_number" => new BadRequestException("The card number is not a valid credit card number."),
                "invalid_expiry_month" => new BadRequestException("The card's expiration month is invalid."),
                "invalid_expiry_year" => new BadRequestException("The card's expiration year is invalid."),
                "invalid_cvc" => new BadRequestException("The card's security code is invalid."),
                "expired_card" => new BadRequestException("The card has expired. Please use a different card."),
                "incorrect_cvc" => new BadRequestException("The card's security code is incorrect."),
                "incorrect_zip" => new BadRequestException("The card's zip code failed validation."),
                "insufficient_funds" => new BadRequestException("The card has insufficient funds to complete the purchase."),
                "country_not_supported" => new BadRequestException("The country of the card is not supported."),
                "authentication_required" => new BadRequestException("This transaction requires authentication."),
                "rate_limit" => new InternalServerException("Too many requests hit the API too quickly. Please slow down your requests."),
                "invalid_request_error" => new BadRequestException(stripeError?.Message ?? "Invalid request."),
                "api_error" => new InternalServerException("Stripe encountered an internal error. Please try again later."),
                "account_already_exists" => new ConflictException("Account with this email already exists."),
                "account_country_invalid_address" => new BadRequestException("Country specified in the address is not currently supported."),
                "account_invalid" => new BadRequestException("Stripe account ID is invalid."),
                "resource_missing" => new NotFoundException("Requested resource is not found."),
                "unsupported_card" => new BadRequestException("The card is not supported in the region."),
                _ => new PaymentException(exception.Message, errorCode, (int)exception.HttpStatusCode)
            };
        }


    }
}

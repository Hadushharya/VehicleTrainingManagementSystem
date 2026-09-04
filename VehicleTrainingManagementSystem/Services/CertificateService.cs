using QRCoder;
using System.Security.Cryptography;

namespace VehicleTrainingManagementSystem.Services
{
    public class CertificateService
    {
        public string GenerateCertificateNumber()
        {
            // 6-digit cryptographically random number, e.g. ABS-2026-483920
            int randomNumber = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return $"ES-{DateTime.Now.Year}-{randomNumber:D6}";
        }

        public string GenerateQrCodeBase64(string verifyUrl)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(verifyUrl, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            byte[] bytes = pngQr.GetGraphic(20);
            return Convert.ToBase64String(bytes);
        }
    }
}
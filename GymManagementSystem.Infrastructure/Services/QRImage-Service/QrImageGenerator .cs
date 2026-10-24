using QRCoder;

namespace GymManagementSystem.Infrastructure.Services.QRImage_Service
{
    public static class QrImageGenerator
    {
        public static byte[] CreatePng(string token)
        {
            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(token, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(data);

            return png.GetGraphic(20);
        }
    }
}

using System.Text;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutomacoesUndDocker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutomationsIDGRController : ControllerBase
    {

        static int messageId = 0;

        [HttpGet("relatorio-estoque/{cnpj}/{data}")]
        public string RelatorioEstoque([FromRoute] string cnpj, [FromRoute] string data)
        {

            var factory = new ConnectionFactory() { HostName = "44.211.234.2", Port = 5672 };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "letterbox",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            messageId++;

            var message = "{\n"
                            + $"    \"relatorio\": \"estoque\",\n"
                            + $"    \"cnpj\": \"{cnpj}\",\n"
                            + $"    \"data\": \"{data}\",\n"
                            + $"    \"id\": \"{messageId}\"\n"
                            + "}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "letterbox", null, body);

            Console.WriteLine($"Send message: {message}");

            return "<html lang=\"pt\">\r\n<head>\r\n" +
                "\t<meta charset=\"utf-8\" />\r\n" +
                "\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\" />\r\n" +
                "\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
                "\t<title></title>\r\n\t<link href='https://fonts.googleapis.com/css?family=Lato:300,400|Montserrat:700' rel='stylesheet' type='text/css'>\r\n" +
                "\t<style>\r\n\t\t@import url(//cdnjs.cloudflare.com/ajax/libs/normalize/3.0.1/normalize.min.css);\r\n" +
                "\t\t@import url(//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css);\r\n" +
                "\t</style>\r\n" +
                "\t<link rel=\"stylesheet\" href=\"https://2-22-4-dot-lead-pages.appspot.com/static/lp918/min/default_thank_you.css\">\r\n" +
                "\t<script src=\"https://2-22-4-dot-lead-pages.appspot.com/static/lp918/min/jquery-1.9.1.min.js\"></script>\r\n" +
                "\t<script src=\"https://2-22-4-dot-lead-pages.appspot.com/static/lp918/min/html5shiv.js\"></script>\r\n" +
                "</head>\r\n" +
                "<body>\r\n" +
                "\t<header class=\"site-header\" id=\"header\">\r\n" +
                "\t\t<h1 class=\"site-header__title\" data-lead-id=\"site-header-title\">THANK YOU!</h1>\r\n" +
                "\t</header>\r\n\r\n\t<div class=\"main-content\">\r\n" +
                "\t\t<i class=\"fa fa-check main-content__checkmark\" id=\"checkmark\"></i>\r\n" +
                "\t\t<p class=\"main-content__body\" data-lead-id=\"main-content-body\">You are contributing to enhance automations in IDGR.</p>\r\n" +
                "\t</div>\r\n\r\n" +
                "\t<footer class=\"site-footer\" id=\"footer\">\r\n" +
                "\t</footer>\r\n" +
                "</body>\r\n" +
                "</html>";
        }

    }
}
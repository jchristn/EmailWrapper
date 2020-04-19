# EmailWrapper

[![NuGet Version](https://img.shields.io/nuget/v/EmailWrapper.svg?style=flat)](https://www.nuget.org/packages/EmailWrapper/) [![NuGet](https://img.shields.io/nuget/dt/EmailWrapper.svg)](https://www.nuget.org/packages/EmailWrapper) 

Simple email wrapper in C# for SMTP and Mailgun targeted to both .NET Core 2.0 and .NET Framework 4.5.2.

## Simple Example
```
using EmailWrapper;

EmailClient em = new EmailClient("mailgunapikey", "mydomain.com");                // Mailgun
EmailClient em = new EmailClient("smtp.domain.com", 25, "user", "pass", false);   // SMTP

Email email = new Email();
email.IsHtml = false;
email.FromAddress = "me@mydomain.com";
email.ReplyAddress = "me@mydomain.com";
email.ToAddress = "me@mydomain.com";
email.CcAddress = "you@mydomain.com";
email.BccAddress = "them@mydomain.com";
email.Subject = "Hello!";
email.Body  "Hello from EmailWrapper!";

if (em.Send(email)) Console.WriteLine("Success!");
else Console.WriteLine("Failure!");
```

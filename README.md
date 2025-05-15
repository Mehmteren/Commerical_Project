# 🛒 E-Commerce Microservices with SAGA Pattern

<div align="center">

![.NET](https://img.shields.io/badge/.NET%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MassTransit](https://img.shields.io/badge/MassTransit-5C2D91?style=for-the-badge&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAABmJLR0QA/wD/AP+gvaeTAAAAB3RJTUUH5AYGEBwYiINDDQAAA1FJREFUKM8FwUtoXGUAB/D/d+93595JJjOTmcmkSZpMH0ObkjQJIa0JpC+hLYqCIEjduBFEcOFCBKELN4ILFy5KRUHQlWvpQgRBaLW0SA1JHzTYJDWT1ySZzOPO3HvP/X7f73eEEKyuruJ/kiRRr9e/n5+f/3hmZua15eXl16MoWu92u00p5bNer/dvGIY/7O/vf7e3t/ebEAJKKaKlpSU8efIEADAzM/P5ysrKp81m8/1Wq3USRRHY6iKNIviVMhzHgZSS9vv9H8fj8Rdra2vfCCEghABrtVp7e3v7m/n5+Y8mJiZ8x3Hw+PEoFfrhMNEGnBbLZSllaTab6fT09KfjOH6nadruKQghYFmWyvO8VCqVCtu2Icsy6vX6G5ZlzZ2dnbWYEOJxwUGpVIDWGiEZOY7z8cLCwptKqZZWKgeIMQZsNhskSRINh8NRmqar29vbH9RqtT9s244BJPj3ASkdgyMJKSU456hWq4sCuCYGgwHq9XqzUqm8m2XZw0aj8QvnvOY4DizLgm3b4JyD0GuyBQBKKZRSYIyhUChcEFrrxLbttFKpPBgMBrS0tPQymJmbm1sdj8fdKIp+lFL+kabpYZ7nfwIIdnc7qFTKEOIqGWMMsiyDMQYiSdQwDKRpKg3DuLNarb5DKQ0dx9Dlcvmt/f39VQA/cV5QQsAJAEIIKKWglIJzDgJI5nn+Uggh8zyvBUFwx7KsuwB+rVarXx0cHHRGo9GvQRBsM8bAGPuXMQZCKQghoJSCGgwGgW3bqFQq9TRNvzQM43aSJJvD4RC+7ztaa8tLh1JKX+R5jjRNkaYpGGNQSgGc8++63e5vAI601icAwrW1tZVer/dwOBySpkGglILSCpTalyEvLwOEIBAEwdd5nt+/2J7dbvduGIZ3GWNrZ2dnL3u9HgDAdV0wxkAoBaEUnDNQQqCVQrvdviWE+EB0Oh1UKhU0Go0aSZLvOef3KpXKzPn5Odrt9nWVhMIwCjrnEEKAsw0wxnAviiJcXFw86vf7H1mWdSdJkmeWZdlCiEs55zDNCiihYNSAbVvI8xx5nsMwjOeDweAYIHj6d1BcX3/n1s/fNr98+m317GnXtG0LSslfTk9Pv2GMDfM8v0HTIFBag3MOSimklC2t9T3f978B8A8mfwB9XLtgrgAAAABJRU5ErkJggg==)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

</div>
---

## 📖 Hakkında

Bu proje, mikro servis mimarisi ve **SAGA Pattern** kullanarak geliştirilmiş bir e-ticaret sistemidir. Sipariş oluşturma süreci **orkestrasyon yaklaşımı** ile yönetilmektedir.

---

## 🏗️ Proje Mimarisi

| Servis | Açıklama |
|--------|----------|
| **Basket.API** | Kullanıcı sepet işlemlerini yönetir. Redis üzerinde sepet verilerini depolar. |
| **Stock.API** | Ürün stok kontrolü ve stok yönetiminden sorumludur. |
| **Payment.API** | Ödeme işlemlerini gerçekleştirir. |
| **Order.API** | Sipariş oluşturma ve yönetimini sağlar. |
| **Users.API** | Kullanıcı bilgilerini yönetir. |
| **Product.API** | Ürün bilgilerini yönetir. |
| **SagaStateMachine.Service** | SAGA orchestrator görevi görür, tüm mikroservisler arasındaki işlem akışını yönetir. |
| **Shared** | Mikroservisler arası iletişim için paylaşılan eventler ve mesajların bulunduğu kütüphanedir. |

---

## 🚀 Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|-----------|----------|
| .NET 8.0 | Ana framework |
| Entity Framework Core | ORM & Database Migration |
| PostgreSQL | Ana veritabanı |
| Redis | Sepet verilerinin depolanması |
| RabbitMQ | Message Broker |
| MassTransit | Message Bus implementasyonu |
| Swagger | API dokümantasyonu |
| Docker | Containerization |

---

## ⚙️ Kurulum

### Gereksinimler

- .NET 8.0 SDK  
- Docker Desktop  
- Visual Studio 
- PostgreSQL (lokal ya da Docker container)  
- Redis (Docker container)  
- RabbitMQ  

### Docker Servislerini Başlat

```bash
# Redis
docker run --name redis-microservices -p 6379:6379 -d redis
```

Proje Ayarları
- Her mikroservisin appsettings.json dosyasında kendi bağlantı dizelerini güncelleyin:
- Her mikroservis için veritabanı migration işlemlerini gerçekleştirin:
```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "Redis": "127.0.0.1:1453"
  },
  "RabbitMQ": "amqps://<username>:<password>@<your-host>/<vhost>",
  "AllowedHosts": "*"
}
```

# Her servis için Package Manager Console'da:
Update-Database

Not: Stock.API için migration işlemine gerek yoktur, proje ilk çalıştırıldığında otomatik olarak tablolar oluşturulacak ve örnek veriler eklenecektir.

🚀 Projeyi Çalıştırma

Docker'ın çalıştığından emin olun.
Visual Studio'da projeyi çalıştırın (F5 veya Ctrl+F5).
Swagger UI üzerinden API endpointlerini test edebilirsiniz.

📚 Mimari Notlar

Redis Kullanımı
Basket.API, sepet verilerini Redis üzerinde depolar. Bu, yüksek performanslı ve geçici veri depolama sağlar.
SAGA Pattern
Proje, dağıtık işlemleri yönetmek için SAGA Pattern kullanmaktadır. SagaStateMachine.Service, tüm işlem akışını orkestre eder ve herhangi bir hata durumunda telafi edici işlemleri (compensating transactions) tetikler.
Event-Driven Mimari
Mikroservisler arası iletişim RabbitMQ üzerinden event-driven yaklaşımla sağlanmaktadır. MassTransit kütüphanesi, event yayınlama ve tüketme işlemlerini yönetmektedir.

## 🔄 SAGA Akış Detayları

Projemiz, dağıtık işlemleri yönetmek için Orkestrasyon tabanlı SAGA Pattern kullanır. Tipik bir sipariş akışı şu adımlardan oluşur:

,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,?

🔄 SAGA Pattern Akış Detayları
Bu proje Orkestrasyon tabanlı SAGA Pattern ile çalışmaktadır. Tipik bir sipariş süreci:

✅ Başarılı Akış
Kullanıcı sepete ürün ekler → ProductAddedToBasketRequestEvent

Stok kontrolü yapılır → StockReservedEvent

Ödeme başlatılır → PaymentCompletedEvent

Sipariş verisi kaydedilir → OrderCompletedEvent

Stok kalıcı olarak azaltılır → StockReducedEvent

❌ Hatalı Senaryolar
Stok Yetersiz:
StockNotReservedEvent → Sipariş iptali ve kullanıcı bilgilendirmesi

Ödeme Başarısız:
PaymentFailedEvent → Rezerve edilen stok geri alınır

OrderFailEvent ile işlem sonlandırılır

🛠️ Geliştirme Notları
Redis: Geçici ama hızlı veri saklama için kullanıldı (Basket API).

RabbitMQ + MassTransit: Tüm event akışı ve mikroservis haberleşmesi için kullanıldı.

SagaStateMachine.Service: Süreçleri orkestre eder, hata yönetimini sağlar.





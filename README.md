# Email

Esse é um projeto de estudos que apresenta uma API para envio de emails, além de dois microserviços responsáveis pelo preparo do email e pelo envio do email por meio da tecnologia SMTP do Google.

## Tecnologias

* Asp.NET
	* Swagger
	* Entity Framework
	* JWT Token
* Docker
* SQL Server

## Arquitetura e escolhas

O projeto foi criado com a inteção de seguir o padrão de Clean Architecture, com as camadas:
 
* Common
* Domain
* Infra
* Api
* MS PrepareEmail
* MS SendEmail



## Como executar

### Primeiro baixe o repositório

```bash
git clone https://github.com/jos3s/MSEmail/

or

gh repo clone jos3s/MSEmail/
```
Acesse os projetos API, MSEmail.PrepareEmail e MSEmail.SendEmail e altere os arquivos appsettings.json configurando a **ConnectionStrings** do banco.

```json
{
  ...
  "ConnectionStrings": {
    "MsEmail": "Server= ;Database= ;User ID= ;Password= ;Integrated Security = True;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  },
  ...
}

```

No projeto da API configure um valor para **TokenSecret**
```json
{
  ...
  "AppSettings": {
    "DefaultUserId": 1,
    "TokenSecret": ""
  }
}

```


No MSEmail.SendEmail é preciso fazer uma configuração a mais, pois para o envio do email é necessário configurar qual email vai ser utilizado como redirecionamento. Para isso pode usar o texto de suporte do google  [aqui](https://support.google.com/accounts/answer/185833).

```json
{
  "AppSettings": {
    "DefaultUserId": 1,
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUserName": "",
    "SmtpPassword": ""
  }
}

```



### Para rodar os projetos

Para rodar os projetos execute os seguintes comandos no terminal ou execute-os pelo Visual Studio:

API

```bash
dotnet run --project .\MsEmail.Api
```


MS.PrepareEmail
```bash
dotnet run --project .\MsEmail.PrepareEmail
```

MS.SendEmail
```bash
dotnet run --project .\MsEmail.SendEmail
```


## Endpoints

Ao rodar pelo Visual Studio, vai abrir a interface do swagger com todos os endpoints e schemas da api.
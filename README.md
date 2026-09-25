# TechStore

TechStore is a full-stack e-commerce platform for selling technology products such as smartphones, laptops, and accessories.

The system provides product browsing and searching, shopping cart, voucher management, order processing, online payment, order tracking, AI-powered product recommendations, and an administration dashboard.

## 🛠️ Technologies

### Backend

* **ASP.NET Core 8 Web API**
* **Entity Framework Core**
* **PostgreSQL**
* **JWT Authentication**
* **HttpOnly Cookies**
* **Repository & Unit of Work**
* **RESTful API**
* **SignalR** for real-time payment status updates

### Frontend

* **Angular 17**
* **TypeScript**
* **SCSS**
* **Bootstrap 5**
* **NgRx**

### Other Technologies & Services

* **SePay** — Online bank transfer payment
* **Google Gemini** — AI product search and recommendation
* **Cloudinary** — Image storage
* **Render** — Deployment
* **NEON** — Cloud Database

---

# ✨ Features

## 👤 Customer

### Authentication & Account

* Register a new account
* Login / Logout
* JWT-based authentication
* Access Token stored in **HttpOnly Cookie**
* Refresh Token stored in **HttpOnly Cookie**
* Refresh Token Rotation
* Automatic Access Token renewal
* Authentication state management
* Authorization based on user roles

### Product

* Browse products
* Search products
* Filter products by:

  * Category
  * Brand
  * Price range
* View product details
* View product images/gallery
* View product variants/options

### Shopping Cart

* Add products to cart
* Update product quantities
* Remove products from cart
* Validate product availability
* Validate stock before checkout

### Voucher

* View available vouchers
* Apply voucher during checkout
* Validate:

  * Voucher status
  * Expiration time
  * Usage limit
  * Minimum order value
  * Customer usage
* Prevent invalid or duplicated voucher usage

### Checkout & Orders

* Enter shipping information
* Select payment method
* Create orders
* Apply vouchers
* Calculate final order amount
* View order history
* View order details
* Track order status

### Online Payment

* Support online bank transfer through **SePay**
* Generate payment information / QR Code
* Receive payment confirmation through webhook
* Verify payment amount
* Prevent duplicate payment processing
* Handle overpayment
* Update order/payment status
* Receive real-time payment status through **SignalR**

### AI Product Assistant

TechStore provides an AI-powered chatbot that helps customers search for and select products.

The chatbot can:

* Understand natural-language product requests
* Extract product search criteria
* Search products based on:

  * Category
  * Brand
  * Price range
  * Usage
  * Games
* Maintain conversation context
* Provide product recommendations
* Handle follow-up questions based on previous interactions
* Validate recommended Product IDs against products returned by the system

Example:

> "I need a gaming laptop under $1000 that can run Elden Ring."

The AI can extract relevant requirements and search TechStore products accordingly.

---

# 👨‍💼 Admin

## Dashboard

* View system statistics
* Manage store data
* Monitor orders and users

## Product Management

* Create products
* Update products
* Delete products
* Manage product information
* Manage product images
* Manage product variants/options
* Manage product stock

## Category Management

* Create categories
* Update categories
* Delete categories
* View categories

## Brand Management

* Create brands
* Update brands
* Delete brands
* View brands

## User Management

* View users
* Manage customer accounts
* Manage user roles
* Enable / disable user accounts

## Order Management

* View all orders
* View order details
* Update order status
* Manage shipping information
* Monitor payment status

## Voucher Management

* Create vouchers
* Update vouchers
* Delete vouchers
* Configure:

  * Discount value
  * Minimum order value
  * Expiration time
  * Usage limits
  * Customer usage restrictions

---

# 🔐 Authentication & Authorization

TechStore uses **JWT Authentication combined with HttpOnly Cookies**.

Instead of exposing authentication tokens to JavaScript or storing them in `localStorage`, the tokens are stored in secure cookies.

### Authentication Flow

```text
Customer
   │
   │ Login
   ▼
Angular
   │
   │ POST /authentication/login
   ▼
ASP.NET Core API
   │
   ├── Validate credentials
   ├── Generate Access Token
   └── Generate Refresh Token
   │
   ▼
HttpOnly Cookies
   │
   ├── access_token
   └── refresh_token
```

The browser automatically sends these cookies with subsequent requests.

### Access Token

The Access Token:

* Is a JWT
* Is stored in an **HttpOnly Cookie**
* Is used to authenticate API requests
* Contains user identity and role claims
* Has a short expiration time

### Refresh Token

The Refresh Token:

* Is stored in an **HttpOnly Cookie**
* Has a longer lifetime
* Is stored securely in the database as a hash
* Is rotated when used
* Can be revoked

### Refresh Token Rotation

When the Access Token expires:

```text
Angular Request
      │
      ▼
API → 401 Unauthorized
      │
      ▼
Refresh Token Request
      │
      ▼
Validate Refresh Token
      │
      ├── Valid
      │     │
      │     ▼
      │  Revoke old token
      │     │
      │     ▼
      │  Generate new token pair
      │
      └── Invalid / Expired
              │
              ▼
          Logout User
```

The system also handles concurrent refresh requests to prevent multiple requests from simultaneously rotating the same Refresh Token.

---

# ⚡ Concurrency Handling

Concurrency is handled in several critical business operations where multiple requests may attempt to modify the same data simultaneously.

## 1. Stock Concurrency

The system prevents overselling when multiple customers attempt to purchase the same product at the same time.

Example:

```text
Stock = 1

Customer A ──┐
              ├──> Checkout
Customer B ──┘
```

Without concurrency control, both requests could potentially purchase the same product.

TechStore validates and updates stock within a transactional operation so that only a valid request can successfully reserve/purchase the available stock.

---

## 2. Stock Reservation

During checkout, stock can be temporarily reserved.

```text
Available Stock
       │
       ▼
Stock Reservation
       │
       ├── Payment successful
       │       ↓
       │    Complete Order
       │
       └── Reservation expired
               ↓
          Release Stock
```

Expired reservations are automatically released so that the stock becomes available again.

---

## 3. Voucher Concurrency

Voucher usage is also protected against concurrent requests.

For example, if a voucher has:

```text
Usage Limit = 1
```

and two customers attempt to use it simultaneously, the system prevents both requests from successfully consuming the same voucher usage.

Voucher usage is validated and persisted as part of the checkout transaction.

---

## 4. Payment Concurrency & Idempotency

Payment webhooks may be delivered more than once.

TechStore therefore does not blindly process every webhook request.

The payment flow includes:

* Payment transaction logging
* Payment validation
* Idempotency handling
* Order status validation
* Transactional order update
* Protection against duplicate payment processing

Example:

```text
SePay Webhook
      │
      ▼
Payment Transaction Log
      │
      ▼
Check Idempotency
      │
      ├── Already processed → Ignore
      │
      └── New transaction
              │
              ▼
       Validate Payment
              │
              ▼
        Update Order
```

This prevents the same payment event from creating or updating an order multiple times.

---

## 5. Refresh Token Concurrency

Multiple API requests may detect an expired Access Token at approximately the same time.

Without concurrency handling:

```text
Request A ──> Refresh Token ──┐
                              ├──> Both rotate the same token
Request B ──> Refresh Token ──┘
```

TechStore handles this situation by protecting Refresh Token rotation so that the same Refresh Token cannot be successfully consumed multiple times.

The system uses transactional processing and database-level concurrency control when updating the Refresh Token.

---

# 🔄 Transaction & Data Consistency

Critical business operations are executed within database transactions.

Examples include:

* Creating an order
* Reserving stock
* Applying voucher usage
* Processing payment
* Updating payment status
* Rotating refresh tokens

The purpose is to ensure that related changes are committed together.

For example:

```text
Create Order
     │
     ├── Create Order Items
     ├── Reserve Stock
     ├── Apply Voucher
     └── Create Payment Information
            │
            ▼
        Transaction
            │
       ┌────┴────┐
       │         │
    Success    Failure
       │         │
    Commit     Rollback
```

If a critical operation fails, the transaction can be rolled back to maintain data consistency.

---

# 💳 Payment Flow

The online payment flow is designed around a payment snapshot and webhook verification.

```text
Customer
   │
   ▼
Create Prepayment Order
   │
   ▼
Payment Snapshot
   │
   ▼
Display QR Code
   │
   ▼
Customer transfers money
   │
   ▼
SePay Webhook
   │
   ▼
Verify Payment
   │
   ├── Invalid → Reject
   │
   └── Valid
         │
         ▼
   Process Order
         │
         ▼
   Update Payment/Order
         │
         ▼
   SignalR Notification
         │
         ▼
   Customer receives result
```

The system also handles cases such as:

* Duplicate webhook events
* Already-paid orders
* Expired orders
* Invalid payment amounts
* Overpayment

---

# 🧠 AI Architecture

The AI chatbot separates natural-language understanding from product retrieval.

```text
User Message
     │
     ▼
AI Context
     │
     ▼
Extract Search Criteria
     │
     ▼
Product Search
     │
     ▼
Product Context
     │
     ▼
Generate Recommendation
     │
     ▼
Validate Product IDs
     │
     ▼
Response
```

The system stores conversation information using:

* `AiConversation`
* `AiConversationMessage`
* `AiConversationContext`

This allows the chatbot to maintain context between messages while avoiding inappropriate reuse of previous criteria when the customer changes product categories.

---

# 🏗️ Backend Architecture

The backend follows a layered architecture with separation between:

```text
TechStore.API
      │
      ▼
TechStore.Services
      │
      ▼
TechStore.Data
      │
      ▼
Database
```

The system applies several common backend patterns and practices, including:

* Repository Pattern
* Unit of Work
* Dependency Injection
* Service Layer
* DTOs
* JWT Authentication
* Database Transactions
* Idempotency
* Concurrency Control
* Background Processing

---

# 🗄️ Main Business Entities

Some of the main entities in the system include:

* User
* Role
* Product
* Category
* Brand
* Product Variant
* Cart
* Order
* Order Item
* Payment
* Payment Transaction
* Payment Snapshot
* Voucher
* Voucher Usage
* Refresh Token
* Stock Reservation
* AI Conversation
* AI Conversation Message
* AI Conversation Context
* Idempotency Key

---

# 🚀 Deployment

The application is deployed using:

* **Frontend:** Angular 17
* **Backend:** ASP.NET Core 8
* **Database:** PostgreSQL
* **Frontend Hosting:** Render
* **Backend Hosting:** Render
* **Image Storage:** Cloudinary
* **Payment:** SePay
* **AI:** Google Gemini

---

# 📌 Key Engineering Features

TechStore is not only a basic CRUD e-commerce application. The system focuses on several real-world backend engineering problems:

* Secure authentication with **JWT + HttpOnly Cookies**
* **Refresh Token Rotation**
* Concurrent refresh request handling
* **Stock Reservation**
* Transactional checkout
* Voucher concurrency control
* Payment webhook processing
* Payment **Idempotency**
* Duplicate webhook protection
* Database transactions
* Real-time payment updates with **SignalR**
* AI-powered product search and recommendation
* Conversation context management
* Background processing for expired stock reservations
* Role-based authorization
* Separation of business logic from API controllers

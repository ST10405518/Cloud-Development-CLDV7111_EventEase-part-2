# Reflective Technical Report: EventEase Application

**Student:** ST10405518  
**Course:** Cloud Development (CLDV7111)  
**Date:** June 2026

---

## 1. System Feature List

### 1.1 Core Features

The EventEase application is a comprehensive venue and event management system designed to streamline the booking process for event organizers and venue managers. The following features have been implemented across Parts 1, 2, and 3 of the project:

#### Venue Management
- **Create Venues:** Add new venues with details including name, location, capacity, and availability status
- **View Venues:** Browse all venues in a card-based layout with images, location, capacity, and availability indicators
- **Edit Venues:** Update venue information including availability status
- **Delete Venues:** Remove venues with protection against deletion if bookings exist
- **Venue Image Upload:** Upload and manage venue images using Azure Blob Storage
- **Venue Availability Status:** Toggle venue availability to control booking permissions

#### Event Management
- **Create Events:** Schedule new events with event type classification, venue assignment, date/time, and descriptions
- **View Events:** Display all events with event type badges, venue information, and event details
- **Edit Events:** Modify event details including event type reclassification
- **Delete Events:** Remove events with protection against deletion if bookings exist
- **Event Type Classification:** Categorize events by type (e.g., Conference, Wedding, Concert, Workshop)
- **Event Search:** Search events by name or description

#### Booking Management
- **Create Bookings:** Book venues for specific events with date/time ranges and customer information
- **View Bookings:** Display all bookings with event details, venue information, customer details, and status
- **Edit Bookings:** Modify booking details with conflict detection
- **Delete Bookings:** Cancel bookings with cascade delete protection
- **Advanced Filtering:** Filter bookings by:
  - Event type
  - Date range (start date to end date)
  - Venue availability status
  - Text search (event name, venue name, customer name)
- **Double Booking Prevention:** Automatic conflict detection to prevent overlapping bookings
- **Booking Status Tracking:** Track booking status (Confirmed, Pending, Cancelled)
- **Calendar View:** Visual calendar representation of all bookings

#### Event Type Management
- **Create Event Types:** Define new event type categories with descriptions and availability status
- **View Event Types:** Browse all event types with availability indicators
- **Edit Event Types:** Update event type details and availability
- **Delete Event Types:** Remove event type categories
- **Event Type Availability:** Control which event types are available for new events

#### Search and Filtering
- **Text Search:** Search across venues, events, and bookings by name, location, or description
- **Advanced Filtering:** Multi-criteria filtering for bookings with date ranges and availability filters
- **Real-time Filter Application:** Instant application of filter criteria

#### User Interface
- **Responsive Design:** Mobile-friendly interface using Bootstrap 5
- **Modern UI Components:** Card-based layouts, badges, icons, and form validation
- **Navigation:** Intuitive navigation with clear menu structure
- **Hero Sections:** Visual headers with icons for each section
- **Form Validation:** Client-side and server-side validation for data integrity

---

## 2. Component Discussion

### 2.1 Azure Services Used

#### Azure SQL Database
**Purpose:** Primary relational database for storing all application data including venues, events, bookings, and event types.

**Why Used:**
- Fully managed relational database service with automatic backups and high availability
- Seamless integration with Entity Framework Core for ORM functionality
- Scalable performance with ability to handle concurrent users
- Built-in security features including firewall rules and encryption
- Cost-effective for the application's data storage needs
- Supports complex queries and relationships required for the booking system

**Alternatives Considered:**
- **Azure Cosmos DB:** Could have been used for its NoSQL capabilities and global distribution, but the relational nature of the data (foreign keys, complex relationships) made SQL Database a better fit
- **Azure Database for PostgreSQL:** Would have provided similar relational capabilities, but SQL Database offered better integration with .NET ecosystem and Entity Framework Core

#### Azure Blob Storage
**Purpose:** Storage for venue images uploaded through the application.

**Why Used:**
- Cost-effective object storage optimized for images and media files
- High durability and availability with geo-redundancy options
- Seamless integration with ASP.NET Core through Azure SDK
- Supports large file sizes and various image formats
- Provides CDN integration for faster content delivery
- Secure access through shared access signatures (SAS) or managed identities

**Alternatives Considered:**
- **Azure Files:** Could have been used for file storage, but Blob Storage is more optimized for images and media files
- **Local File System:** Would have been simpler but lacks scalability, backup, and accessibility across multiple instances
- **Third-party CDN services:** Would have added complexity and cost without significant benefits for this use case

#### Azure Web App
**Purpose:** Hosting platform for the ASP.NET Core application.

**Why Used:**
- Fully managed platform-as-a-service (PaaS) with automatic scaling
- Continuous deployment support through Git integration
- Built-in load balancing and high availability
- Easy integration with other Azure services
- Supports multiple deployment slots for staging
- Automatic OS patching and maintenance
- Cost-effective with pay-as-you-go pricing

**Alternatives Considered:**
- **Azure Container Apps:** Would have provided container-based deployment, but added complexity without significant benefits for this application
- **Azure Virtual Machines:** Would have given more control but required more maintenance and management overhead
- **Azure Static Web Apps:** Not suitable as the application requires server-side processing and database connectivity

### 2.2 Technologies Used

#### ASP.NET Core 8.0
**Why Used:**
- Modern, high-performance web framework from Microsoft
- Cross-platform support (Windows, Linux, macOS)
- Built-in dependency injection and middleware pipeline
- Excellent support for RESTful APIs and MVC patterns
- Strong integration with Entity Framework Core
- Comprehensive security features (authentication, authorization, CSRF protection)
- Active community and extensive documentation

**Alternatives Considered:**
- **Node.js with Express:** Would have provided JavaScript-based development but lacks the strong typing and structure of .NET
- **Python with Django:** Good for rapid development but less performant for high-traffic applications
- **Java Spring Boot:** Robust framework but with steeper learning curve and more verbose configuration

#### Entity Framework Core
**Why Used:**
- Object-relational mapper (ORM) that simplifies database operations
- Code-first approach with automatic migrations
- LINQ support for type-safe database queries
- Change tracking and automatic updates
- Support for complex relationships and navigation properties
- Database-agnostic design with provider model
- Integration with dependency injection

**Alternatives Considered:**
- **Dapper:** Lightweight micro-ORM but requires more manual SQL writing
- **NHibernate:** Mature ORM but with more complex configuration
- **Raw ADO.NET:** Would provide maximum control but requires significant boilerplate code

#### Bootstrap 5
**Why Used:**
- Popular CSS framework for responsive design
- Pre-built components (cards, forms, buttons, navigation)
- Mobile-first approach with grid system
- Customizable theming with Sass support
- Extensive documentation and community support
- Consistent design patterns across the application

**Alternatives Considered:**
- **Tailwind CSS:** Utility-first approach but requires more HTML markup
- **Bulma:** Lightweight alternative but with fewer components
- **Custom CSS:** Would provide complete control but requires more development time

#### C# Programming Language
**Why Used:**
- Strongly typed language with compile-time error checking
- Modern language features (LINQ, async/await, pattern matching)
- Excellent integration with .NET ecosystem
- Object-oriented programming support
- Garbage collection and memory management
- Extensive standard library

**Alternatives Considered:**
- **F#:** Functional programming paradigm but less widely used
- **Visual Basic:** Legacy language with declining support
- **TypeScript:** Would require JavaScript runtime environment

---

## 3. Project Reflection

### 3.1 Personal Experience and Challenges

#### Part 1: Foundation and Core Functionality
The initial phase of the project focused on establishing the core infrastructure and basic CRUD operations for venues, events, and bookings. This phase provided valuable experience in:

- **Database Design:** Designing the relational schema with proper relationships and constraints
- **ORM Implementation:** Learning Entity Framework Core's code-first approach and migration system
- **Azure Integration:** Setting up Azure SQL Database and configuring connection strings
- **Basic CRUD Operations:** Implementing create, read, update, and delete functionality
- **Image Storage:** Implementing Azure Blob Storage for venue images

**Challenges Faced:**
- Initial confusion with Entity Framework Core's navigation properties and eager loading
- Understanding Azure SQL Database connection security and firewall rules
- Configuring Blob Storage authentication and SAS tokens
- Managing image upload and display in the UI

#### Part 2: Search Functionality and Calendar View
The second phase enhanced the application with search capabilities and a calendar view for bookings. This phase introduced:

- **Search Implementation:** Adding text-based search across multiple entities
- **Calendar Integration:** Creating a visual calendar representation of bookings
- **UI Enhancements:** Improving the user interface with better layouts and components
- **Data Validation:** Adding more robust validation rules

**Challenges Faced:**
- Implementing efficient search queries that work across related entities
- Designing an intuitive calendar view that displays booking information clearly
- Handling edge cases in search (empty results, special characters)
- Ensuring calendar view updates correctly when bookings are modified

#### Part 3: Advanced Filtering and Event Type Classification
The final phase added sophisticated filtering capabilities and event type classification. This phase required:

- **Database Schema Changes:** Adding new tables and fields while maintaining data integrity
- **Advanced Filtering:** Implementing multi-criteria filtering with date ranges and boolean filters
- **Event Type Management:** Creating a complete CRUD system for event types
- **Migration Management:** Handling database migrations without data loss

**Challenges Faced:**
- Designing the EventType table and its relationship with Events
- Implementing complex filtering logic in the controller
- Updating all related views to display new information
- Managing foreign key constraints during migrations
- Ensuring backward compatibility with existing data

### 3.2 Lessons Learned

#### Technical Lessons
1. **Database Schema Evolution:** Learned the importance of planning database changes carefully and using migrations to manage schema evolution without data loss
2. **ORM Best Practices:** Understood the value of eager loading (Include) versus lazy loading for performance optimization
3. **Azure Service Integration:** Gained practical experience in integrating multiple Azure services (SQL Database, Blob Storage, Web App) in a cohesive application
4. **Validation Strategies:** Learned to implement both client-side and server-side validation for robust data integrity
5. **UI/UX Design:** Appreciated the importance of consistent design patterns and user-friendly interfaces
6. **Error Handling:** Developed strategies for handling database errors, connection failures, and user input errors gracefully

#### Project Management Lessons
1. **Incremental Development:** Breaking down complex features into smaller, manageable tasks made the project more approachable
2. **Testing Importance:** Learned the value of testing each feature thoroughly before moving to the next
3. **Documentation:** Keeping track of design decisions and implementation details helped in maintaining consistency
4. **Version Control:** Using Git effectively to manage changes and revert when necessary
5. **Time Management:** Balancing feature implementation with documentation and testing

#### Cloud Development Lessons
1. **Service Selection:** Understanding when to use different Azure services based on requirements
2. **Security Considerations:** Implementing proper security measures for database connections and file storage
3. **Scalability Planning:** Designing the application to handle growth in users and data
4. **Cost Management:** Being aware of Azure service costs and optimizing resource usage
5. **Deployment Strategies:** Learning different deployment approaches and their trade-offs

### 3.3 Current Understanding of Cloud-Based Applications

Through this project, I have developed a comprehensive understanding of designing, developing, and architecting cloud-based applications:

#### Design Principles
- **Scalability:** Designing applications that can scale horizontally to handle increased load
- **Availability:** Ensuring high availability through redundant services and proper error handling
- **Security:** Implementing defense-in-depth security with authentication, authorization, and data encryption
- **Maintainability:** Writing clean, modular code that is easy to maintain and extend
- **Performance:** Optimizing database queries, caching strategies, and resource utilization

#### Architecture Patterns
- **N-Tier Architecture:** Separating presentation, business logic, and data access layers
- **Service-Oriented Architecture:** Using Azure services as building blocks for application functionality
- **Microservices Considerations:** Understanding when to use monolithic vs. microservices architecture
- **Event-Driven Architecture:** Appreciating the role of events in decoupling system components

#### Development Practices
- **DevOps Integration:** Understanding the importance of continuous integration and deployment
- **Infrastructure as Code:** Learning to manage infrastructure through code rather than manual configuration
- **Monitoring and Logging:** Implementing proper logging for debugging and performance monitoring
- **Testing Strategies:** Developing comprehensive testing approaches for cloud applications

#### Cloud-Native Concepts
- **Serverless Computing:** Understanding when to use serverless services for cost optimization
- **Containerization:** Appreciating the role of containers in application deployment
- **API Design:** Designing RESTful APIs that are consumable by various clients
- **Data Management:** Handling data consistency, backup, and recovery in cloud environments

#### Future Considerations
- **Global Deployment:** Understanding multi-region deployment for global applications
- **Hybrid Cloud:** Integrating on-premises resources with cloud services
- **AI Integration:** Leveraging Azure AI services for enhanced functionality
- **IoT Integration:** Connecting IoT devices to cloud applications for data collection and analysis

---

## 4. Conclusion

The EventEase project has been a comprehensive learning experience that has significantly enhanced my understanding of cloud-based application development. From the initial setup of Azure services to the implementation of advanced filtering and event type classification, each phase of the project has built upon the previous one, creating a fully functional venue and event management system.

The project has provided practical experience in:
- Azure service integration (SQL Database, Blob Storage, Web App)
- ASP.NET Core MVC development with Entity Framework Core
- Database design and migration management
- Responsive web development with Bootstrap 5
- Advanced search and filtering implementation
- Image storage and management in the cloud

The challenges encountered throughout the project have been valuable learning opportunities, teaching me problem-solving skills and best practices in cloud development. The lessons learned from this project will be applicable to future cloud-based projects and have prepared me for more complex application development scenarios.

This project has demonstrated the power and flexibility of Azure services in building modern, scalable web applications. The combination of Azure SQL Database for relational data, Blob Storage for media files, and Web App for hosting provides a robust foundation for cloud-based applications.

Overall, the EventEase project has been a successful implementation of a cloud-based venue and event management system, meeting all the requirements specified in Parts 1, 2, and 3 of the assignment. The experience gained from this project will be invaluable in my future career as a cloud developer.

---

## References

### Code Attribution
- **ASP.NET Core Documentation:** https://docs.microsoft.com/en-us/aspnet/core
- **Entity Framework Core Documentation:** https://docs.microsoft.com/en-us/ef/core
- **Azure SQL Database Documentation:** https://docs.microsoft.com/en-us/azure/sql-database
- **Azure Blob Storage Documentation:** https://docs.microsoft.com/en-us/azure/storage/blobs
- **Bootstrap Documentation:** https://getbootstrap.com/docs/5.0

### Traditional References
- Microsoft. (2024). *ASP.NET Core Documentation*. Microsoft Docs.
- Microsoft. (2024). *Entity Framework Core Documentation*. Microsoft Docs.
- Microsoft. (2024). *Azure SQL Database Documentation*. Microsoft Docs.
- Microsoft. (2024). *Azure Blob Storage Documentation*. Microsoft Docs.
- The Bootstrap Team. (2024). *Bootstrap 5 Documentation*. Bootstrap.

---

**End of Report**

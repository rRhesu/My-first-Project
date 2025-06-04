# Simple Inventory Management System

This is a **Simple Inventory Management System** built in Visual Basic .NET (VB.NET) for the console. It allows users to manage inventory items, register admin users, and keep an audit trail of key actions. The system stores inventory data in a CSV file and provides user authentication with support for admin accounts.

## Features

- **User Authentication**: Requires users to log in before accessing the system. Supports admin user registration.
- **Inventory Management**: Add, view, update, and delete inventory items.
- **Reporting**: View low stock warnings; placeholder for sales trends.
- **Audit Trail**: Logs important actions such as logins, item modifications, and user registrations.
- **Data Persistence**: Inventory is stored in a CSV file (`inventory.csv`). Audit logs are stored in `audit_trail.txt`.

## Usage

### Running the Program

1. **Open the project** in Visual Studio or any VB.NET compatible IDE.
2. **Build and run** the project.
3. At first run, the program ensures an admin user exists. If not, you'll be prompted to create one.
4. **Log in** with your username and password.
5. Use the menu to manage the inventory and users.

### Main Menu Options

1. **Add Item**: Add a new item to the inventory.
2. **View Inventory**: See all items in the inventory.
3. **Update Item**: Update an existing item's details.
4. **Delete Item**: Remove an item from the inventory.
5. **Reports**:
    - Low Stock Warnings: Lists items with low stock.
    - Sales Trends: (Coming soon)
6. **Register new admin user**: Add another admin account.
7. **Show Audit Trail**: Display the audit log of system actions.
8. **Exit**: Save data and exit the program.

### File Structure

- `Program.vb` - Main module containing the entry point, menu, and core functionality.
- `UserAuth.vb` - Handles user authentication and registration. *(Required for full functionality)*
- `AuditTrail.vb` - Handles logging of actions to the audit trail. *(Required for full functionality)*
- `inventory.csv` - Stores inventory items.
- `audit_trail.txt` - Stores audit logs.

## Requirements

- .NET Framework (suitable for VB.NET console applications)
- Windows OS (for console input/output)
- No database required; uses local CSV and TXT files for storage.

## Getting Started

1. Download or clone the repository.
2. Ensure all required files (`Program.vb`, `UserAuth.vb`, `AuditTrail.vb`) are present in your project.
3. Build and run the project.
4. Follow the prompts in the console.

## Notes

- The **sales trends report** feature is a placeholder and not yet implemented.
- Password input is masked for security in the console interface.
- Only admin users can register new admin accounts.

## License

This project is provided for educational purposes. You may use, modify, or distribute it as you wish.

---

**Author:** [rRhesu](https://github.com/rRhesu)

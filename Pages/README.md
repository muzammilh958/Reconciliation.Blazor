# Pages Folder Structure

## Overview
The Pages folder is organized by feature and functionality to maintain clean code organization.

## Folder Structure

### `/Auth`
Contains all authentication-related pages:
- **Login.razor** - User login page (`/login`)
- **SignUp.razor** - User registration page (`/signup`)

**Route Pattern:** `/auth/*` (optional, current routes are at root level)

### `/Dashboard`
Dashboard and main application pages (future pages).
**Route Pattern:** `/dashboard/*`

### `/Components`
Reusable UI components (future components).

### Root Level Pages
- **Home.razor** - Home/landing page (`/`)
- **Counter.razor** - Counter demo page (`/counter`)
- **Weather.razor** - Weather demo page (`/weather`)
- **NotFound.razor** - 404 error page

## CSS Files
Located in `wwwroot/css/`:
- **login.css** - Styles for Login page
- **signup.css** - Styles for SignUp page

## Adding New Pages

### Adding an Auth Page
Create a new file in `Pages/Auth/NewPage.razor`:
```razor
@page "/your-route"
@layout BlankLayout

<!-- Your content -->
```

### Adding a Dashboard Page
Create a new file in `Pages/Dashboard/NewPage.razor`:
```razor
@page "/dashboard/your-route"
@layout MainLayout

<!-- Your content -->
```

### Adding CSS for a Page
Create a file in `wwwroot/css/your-page.css` and link it in the page:
```html
<link href="css/your-page.css" rel="stylesheet" />
```

## Navigation Links
- Login: `/login`
- Sign Up: `/signup`
- Home: `/`
- Dashboard: `/dashboard/*`

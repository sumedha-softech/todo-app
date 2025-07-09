# ToDo App (React + Vite)

A modern ToDo application built with React, Vite, and CoreUI, featuring group-based task management, starred tasks, and a responsive UI.

## Features

- **Task Groups:** Organize tasks into groups. Create, rename, delete, and toggle visibility of groups.
- **Task Management:** Add, edit, delete, and complete tasks. Move tasks between groups or create a new group on the fly.
- **Starred Tasks:** Mark tasks as starred for quick access. View all starred tasks in a dedicated view.
- **Responsive Sidebar:** Navigation with CoreUI sidebar, including group toggles and quick actions.
- **Dark/Light/Auto Theme:** Switch between color modes from the header.
- **Persistent State:** Uses React Context for global state management.
- **API Integration:** Communicates with backend via RESTful API endpoints (proxied via Vite).
- **Modals:** Add or edit tasks and groups using modal dialogs.
- **Feedback:** User-friendly error messages and empty/completed state illustrations.

## Project Structure

```
todoapp.client/
├── public/
│   ├── manifest.json
│   └── vite.svg
├── src/
│   ├── api/           # API calls for tasks, groups, and auth
│   ├── Components/    # All React components (AppSidebar, Dashboard, GroupCard, etc.)
│   ├── global/        # Context and helpers
│   ├── Hooks/         # Custom hooks (e.g., useTaskEvents)
│   ├── Layout/        # Layout components
│   ├── scss/          # SCSS styles (CoreUI, custom, vendors)
│   ├── _nav.jsx       # Sidebar navigation config
│   ├── App.jsx        # Main App component
│   ├── main.jsx       # Entry point
│   └── routes.jsx     # Route definitions
├── index.html
├── package.json
├── vite.config.js
├── eslint.config.js
└── README.md
```

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) (v18+ recommended)
- [npm](https://www.npmjs.com/) (comes with Node.js)
- Backend API (ASP.NET Core or compatible, see proxy config in [vite.config.js](./vite.config.js))

### Installation

1. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   cd todoapp.client

2. **Install dependencies:**
   npm install or npm i

3. **Run the development server:**
   npm run dev
   The app will be available at https://localhost:56115 (see [vite.config.js](./vite.config.js))

4. **Build for production:**
   npm run build

## API Proxy

API requests to **/api/TaskGroups**, **/api/Tasks**, and **/api/Auth** are proxied to the backend server as configured in [vite.config.js](./vite.config.js).

## Customization

- **Sidebar Navigation:** Edit [_nav.jsx](./src/_nav.jsx) to customize sidebar items.

- **Theme & Styles:** Modify SCSS files in [scss](./src/scss) for custom styles.

- **Context:** Global state is managed in [MyContext.jsx](./src/global/MyContext.jsx)

## License

This project is licensed under the MIT License.

---
Created with ❤️ using React, Vite, and CoreUI.
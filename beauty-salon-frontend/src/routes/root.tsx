import React from "react";
import { Outlet } from "react-router-dom";

const Root: React.FC = () => {
    return (
        <div>
            <header>
                <h1>Beauty Salon App</h1>
            </header>
            <main>
                <Outlet />
            </main>
        </div>
    );
};

export default Root;
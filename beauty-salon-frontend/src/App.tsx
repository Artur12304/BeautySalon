import React from 'react';
import { Outlet } from 'react-router-dom';

const App: React.FC = () => {
  return (
    <div>
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default App;

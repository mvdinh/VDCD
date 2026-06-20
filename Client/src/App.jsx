import React, { useState, useEffect } from 'react';
import { Routes, Route, NavLink, Navigate } from 'react-router-dom';
import './App.css';

import Products from './components/Products';
import Sales from './components/Sales';
import Statistics from './components/Statistics';
import Login from './components/Login';

function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      setUser(JSON.parse(savedUser));
    }
  }, []);

  const handleLogin = (loggedInUser) => {
    setUser(loggedInUser);
  };

  const handleLogout = () => {
    localStorage.removeItem('user');
    setUser(null);
  };

  if (!user) {
    return <Login onLogin={handleLogin} />;
  }

  return (
    <div className="app-layout">
      <nav className="navbar">
        <div className="nav-container" style={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
          <div style={{ display: 'flex', alignItems: 'center' }}>
            <div className="nav-brand" style={{ marginRight: '20px' }}>
              TEST BACKEND
            </div>
            <div className="nav-links" style={{ display: 'flex', gap: '15px' }}>
              <NavLink to="/products" className={({isActive}) => isActive ? "nav-link active" : "nav-link"}>Products</NavLink>
              <NavLink to="/sales" className={({isActive}) => isActive ? "nav-link active" : "nav-link"}>Sales</NavLink>
              <NavLink to="/statistics" className={({isActive}) => isActive ? "nav-link active" : "nav-link"}>Statistics</NavLink>
            </div>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
            <span style={{ color: 'white', fontSize: '14px' }}>Chào, {user.username}</span>
            <button onClick={handleLogout} className="btn btn-secondary" style={{ padding: '6px 12px', fontSize: '12px', cursor: 'pointer' }}>Đăng xuất</button>
          </div>
        </div>
      </nav>

      <main className="main-content">
        <Routes>
          <Route path="/" element={<Navigate to="/products" replace />} />
          <Route path="/products" element={<Products user={user} />} />
          <Route path="/sales" element={<Sales user={user} />} />
          <Route path="/statistics" element={<Statistics />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;


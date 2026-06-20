import React, { useState } from 'react';

const Login = ({ onLogin }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async (e) => {
    e.preventDefault();
    if (username && password) {
      try {
        const response = await fetch('https://localhost:7231/api/v1/Users/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            userName: username,
            password: password
          })
        });

        if (response.ok) {
          const data = await response.json();
          
          const loggedInUser = {
            username: data.userName,
            userId: data.userId,
            role: data.role
          };
          
          localStorage.setItem('user', JSON.stringify(loggedInUser));
          onLogin(loggedInUser);
        } else {
          alert('Tên đăng nhập hoặc mật khẩu không đúng!');
        }
      } catch (error) {
        console.error("Login error:", error);
        alert('Có lỗi xảy ra khi kết nối đến server!');
      }
    } else {
      alert('Vui lòng nhập tên đăng nhập và mật khẩu!');
    }
  };

  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', backgroundColor: '#f0f2f5' }}>
      <div className="card" style={{ padding: '40px', width: '100%', maxWidth: '400px', backgroundColor: 'white', borderRadius: '8px', boxShadow: '0 4px 12px rgba(0,0,0,0.1)' }}>
        <h2 style={{ textAlign: 'center', marginBottom: '24px', color: '#333' }}>Đăng Nhập</h2>
        <form onSubmit={handleLogin} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          <div>
            <label style={{ fontWeight: 'bold', marginBottom: '8px', display: 'block' }}>Tên đăng nhập</label>
            <input 
              required
              type="text" 
              className="form-control" 
              value={username} 
              onChange={(e) => setUsername(e.target.value)} 
              placeholder="Nhập tên đăng nhập"
              style={{ width: '100%', padding: '10px 12px', border: '1px solid #ccc', borderRadius: '4px' }}
            />
          </div>
          <div>
            <label style={{ fontWeight: 'bold', marginBottom: '8px', display: 'block' }}>Mật khẩu</label>
            <input 
              required
              type="password" 
              className="form-control" 
              value={password} 
              onChange={(e) => setPassword(e.target.value)} 
              placeholder="Nhập mật khẩu"
              style={{ width: '100%', padding: '10px 12px', border: '1px solid #ccc', borderRadius: '4px' }}
            />
          </div>
          <button type="submit" className="btn btn-primary" style={{ padding: '12px', fontSize: '16px', marginTop: '16px', cursor: 'pointer', backgroundColor: '#0d6efd', color: 'white', border: 'none', borderRadius: '4px' }}>
            Đăng Nhập
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;

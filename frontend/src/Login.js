import React, { useEffect, useState } from "react";
import "./index.css"; // or "./Login.css" if you keep styles separate

const Login = () => {
  const [showAnimation, setShowAnimation] = useState(true);

  useEffect(() => {
    // Hide animation after 3 seconds
    const timer = setTimeout(() => {
      setShowAnimation(false);
    }, 3000);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="login-bg">
      {showAnimation ? (
        <div className="lamp-intro">
          <div className="lamp">
            <div className="lamp-shade"></div>
            <div className="lamp-stand"></div>
            <div className="lamp-base"></div>
          </div>
          <h2 className="lamp-text">Welcome Back!</h2>
        </div>
      ) : (
        <div className="login-container fade-in">
          <h2>Login</h2>
          <input type="text" placeholder="Email" />
          <input type="password" placeholder="Password" />
          <button className="login-btn">Login</button>
          <p>
            Don’t have an account? <a href="#">Register</a>
          </p>
        </div>
      )}
    </div>
  );
};

export default Login;

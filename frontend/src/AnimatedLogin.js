import React, { useState } from "react";
import "./AnimatedLogin.css";

const AnimatedLogin = () => {
  const [employeeId, setEmployeeId] = useState("");
  const [message, setMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);
  const [isRegistering, setIsRegistering] = useState(false);
  const [registerData, setRegisterData] = useState({
    name: "",
    email: "",
  });

  // Store valid employee IDs
  const [validEmployeeIds, setValidEmployeeIds] = useState([
    "EMP123",
    "EMP456",
    "EMP789",
  ]);

  // Handle punch-in validation
  const handlePunchIn = (e) => {
    e.preventDefault();
    if (validEmployeeIds.includes(employeeId.trim().toUpperCase())) {
      setMessage("✅ You have successfully punched in!");
      setIsSuccess(true);
    } else {
      setMessage("❌ Check your credentials.");
      setIsSuccess(false);
    }
  };

  // Handle registration (auto-generate Employee ID)
  const handleRegisterSubmit = (e) => {
    e.preventDefault();

    if (registerData.name.trim() && registerData.email.trim()) {
      const newId = `EMP${Math.floor(100 + Math.random() * 900)}`; // Random ID between EMP100–EMP999

      setValidEmployeeIds([...validEmployeeIds, newId]);

      setMessage(
        `🎉 Registration Successful! Your Employee ID is ${newId}. Use it to Punch In.`
      );
      setIsRegistering(false);
      setRegisterData({ name: "", email: "" });
      setIsSuccess(true);
    } else {
      setMessage("❌ Please fill all fields before registering.");
      setIsSuccess(false);
    }
  };

  return (
    <div className="animated-bg">
      <div className="login-container">
        <div className="login-box">
          {!isRegistering ? (
            <>
              <h2>Employee Punch In</h2>
              <p className="subtitle">Enter your Employee ID to Punch In</p>
              <form onSubmit={handlePunchIn}>
                <div className="input-group">
                  <input
                    type="text"
                    placeholder="Employee ID"
                    value={employeeId}
                    onChange={(e) => setEmployeeId(e.target.value)}
                    required
                  />
                </div>

                <button type="submit" className="login-btn">
                  Punch In 🚀
                </button>

                {message && (
                  <p className={`message ${isSuccess ? "success" : "error"}`}>
                    {message}
                  </p>
                )}

                <p className="register-text">
                  New Employee?{" "}
                  <span onClick={() => setIsRegistering(true)}>Register here</span>
                </p>
              </form>
            </>
          ) : (
            <>
              <h2>Register Employee</h2>
              <p className="subtitle">Fill in your details below</p>
              <form onSubmit={handleRegisterSubmit}>
                <div className="input-group">
                  <input
                    type="text"
                    placeholder="Full Name"
                    value={registerData.name}
                    onChange={(e) =>
                      setRegisterData({ ...registerData, name: e.target.value })
                    }
                    required
                  />
                </div>

                <div className="input-group">
                  <input
                    type="email"
                    placeholder="Email"
                    value={registerData.email}
                    onChange={(e) =>
                      setRegisterData({ ...registerData, email: e.target.value })
                    }
                    required
                  />
                </div>

                <button type="submit" className="login-btn">
                  Register ✨
                </button>

                <p
                  className="register-text"
                  onClick={() => setIsRegistering(false)}
                >
                  Already registered? <span>Go back to Punch In</span>
                </p>
              </form>
            </>
          )}
        </div>
      </div>

      {/* Floating background shapes */}
      <div className="shape shape1"></div>
      <div className="shape shape2"></div>
      <div className="shape shape3"></div>
      <div className="shape shape4"></div>
      <div className="shape shape5"></div>
    </div>
  );
};

export default AnimatedLogin;

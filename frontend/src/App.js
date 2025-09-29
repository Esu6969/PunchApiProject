import React, { useState } from "react";

function App() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [employeeId, setEmployeeId] = useState("");
  const [actionType, setActionType] = useState("PunchIn");
  const [message, setMessage] = useState("");

  // Register employee
  const handleRegister = async (e) => {
    e.preventDefault();
    const res = await fetch("http://localhost:5031/api/punch/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name, email }),
    });
    const data = await res.json();
    setMessage(data.message);
    setEmployeeId(data.employee?.id || "");
  };

  // Punch in/out
  const handlePunch = async (e) => {
    e.preventDefault();
    const res = await fetch("http://localhost:5031/api/punch/punch", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ employeeId: Number(employeeId), actionType }),
    });
    const data = await res.json();
    setMessage(data.message);
  };

  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #f8fafc 0%, #e0e7ff 100%)",
      display: "flex",
      alignItems: "center",
      justifyContent: "center"
    }}>
      <div style={{
        background: "#fff",
        borderRadius: "16px",
        boxShadow: "0 4px 24px #b6b6b6",
        padding: "32px 32px 24px 32px",
        maxWidth: 420,
        width: "100%"
      }}>
        <h2 style={{
          textAlign: "center",
          color: "#3b82f6",
          fontWeight: 700,
          marginBottom: 24,
          letterSpacing: "1px"
        }}>Punch API Demo</h2>
        <form onSubmit={handleRegister} style={{ marginBottom: 32 }}>
          <h4 style={{ color: "#6366f1", marginBottom: 12 }}>Register Employee</h4>
          <input
            type="text"
            placeholder="Name"
            value={name}
            required
            onChange={(e) => setName(e.target.value)}
            style={{
              width: "100%",
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: "8px",
              border: "1px solid #d1d5db",
              fontSize: "1em"
            }}
          />
          <input
            type="email"
            placeholder="Email"
            value={email}
            required
            onChange={(e) => setEmail(e.target.value)}
            style={{
              width: "100%",
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: "8px",
              border: "1px solid #d1d5db",
              fontSize: "1em"
            }}
          />
          <button type="submit" style={{
            width: "100%",
            background: "#6366f1",
            color: "#fff",
            border: "none",
            borderRadius: "8px",
            padding: "12px 0",
            fontWeight: 600,
            fontSize: "1em",
            cursor: "pointer",
            boxShadow: "0 2px 8px #d1d5db"
          }}>Register</button>
        </form>

        <form onSubmit={handlePunch}>
          <h4 style={{ color: "#6366f1", marginBottom: 12 }}>Punch In/Out</h4>
          <input
            type="text"
            placeholder="Employee ID"
            value={employeeId}
            required
            onChange={(e) => setEmployeeId(e.target.value)}
            style={{
              width: "100%",
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: "8px",
              border: "1px solid #d1d5db",
              fontSize: "1em"
            }}
          />
          <select
            value={actionType}
            onChange={(e) => setActionType(e.target.value)}
            style={{
              width: "100%",
              marginBottom: 12,
              padding: "10px 12px",
              borderRadius: "8px",
              border: "1px solid #d1d5db",
              fontSize: "1em"
            }}
          >
            <option value="PunchIn">Punch In</option>
            <option value="PunchOut">Punch Out</option>
          </select>
          <button type="submit" style={{
            width: "100%",
            background: "#3b82f6",
            color: "#fff",
            border: "none",
            borderRadius: "8px",
            padding: "12px 0",
            fontWeight: 600,
            fontSize: "1em",
            cursor: "pointer",
            boxShadow: "0 2px 8px #d1d5db"
          }}>Submit</button>
        </form>

        {message && (
          <div style={{
            background: "#d1fae5",
            color: "#065f46",
            padding: "18px",
            marginTop: "28px",
            borderRadius: "10px",
            boxShadow: "0 2px 8px #ccc",
            fontWeight: "bold",
            fontSize: "1.1em",
            textAlign: "center",
            border: "1px solid #10b981"
          }}>
            {message}
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
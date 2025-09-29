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
    <div style={{ maxWidth: 400, margin: "40px auto", fontFamily: "sans-serif" }}>
      <h2>Punch API Demo</h2>
      <form onSubmit={handleRegister} style={{ marginBottom: 20 }}>
        <h4>Register Employee</h4>
        <input
          type="text"
          placeholder="Name"
          value={name}
          required
          onChange={(e) => setName(e.target.value)}
          style={{ width: "100%", marginBottom: 8 }}
        />
        <input
          type="email"
          placeholder="Email"
          value={email}
          required
          onChange={(e) => setEmail(e.target.value)}
          style={{ width: "100%", marginBottom: 8 }}
        />
        <button type="submit" style={{ width: "100%" }}>Register</button>
      </form>

      <form onSubmit={handlePunch}>
        <h4>Punch In/Out</h4>
        <input
          type="text"
          placeholder="Employee ID"
          value={employeeId}
          required
          onChange={(e) => setEmployeeId(e.target.value)}
          style={{ width: "100%", marginBottom: 8 }}
        />
        <select
          value={actionType}
          onChange={(e) => setActionType(e.target.value)}
          style={{ width: "100%", marginBottom: 8 }}
        >
          <option value="PunchIn">Punch In</option>
          <option value="PunchOut">Punch Out</option>
        </select>
        <button type="submit" style={{ width: "100%" }}>Submit</button>
      </form>

      {message && (
        <div style={{
          background: "#e0ffe0",
          color: "#222",
          padding: "16px",
          marginTop: "20px",
          borderRadius: "8px",
          boxShadow: "0 2px 8px #ccc",
          fontWeight: "bold",
          fontSize: "1.1em",
          textAlign: "center"
        }}>
          {message}
        </div>
      )}
    </div>
  );
}

export default App;
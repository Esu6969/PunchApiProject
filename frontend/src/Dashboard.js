import React, { useEffect, useState, useRef } from "react";

function Dashboard() {
  const [employee, setEmployee] = useState(null);
  const [loading, setLoading] = useState(true);
  const [timer, setTimer] = useState(0); // seconds
  const [timerActive, setTimerActive] = useState(false);
  const [workMessage, setWorkMessage] = useState("");
  const intervalRef = useRef(null);

  useEffect(() => {
    // Get employee from localStorage (set after login)
    const emp = localStorage.getItem("employee");
    if (emp) {
      setEmployee(JSON.parse(emp));
      setLoading(false);
      // Start timer on mount (simulate punch in)
      setTimerActive(true);
    } else {
      // Optionally, fetch from backend if you have a token
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    if (timerActive) {
      intervalRef.current = setInterval(() => {
        setTimer((prev) => prev + 1);
      }, 1000);
    } else if (!timerActive && intervalRef.current) {
      clearInterval(intervalRef.current);
    }
    return () => clearInterval(intervalRef.current);
  }, [timerActive]);

  const handlePunchOut = async () => {
    setTimerActive(false);
    // Calculate hours and minutes
    const hours = Math.floor(timer / 3600);
    const minutes = Math.floor((timer % 3600) / 60);
    // Send punch out to backend
    await fetch("http://localhost:5031/api/punch/punch", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ employeeId: employee.id, actionType: "PunchOut" }),
    });
    setWorkMessage(`You worked for ${hours} hrs and ${minutes} min`);
    // Remove employee from localStorage and redirect to login
    localStorage.removeItem("employee");
    setTimeout(() => {
      window.location.href = "/";
    }, 2000); // Show message for 2 seconds, then redirect
  };

  if (loading) return <div style={{ textAlign: "center", marginTop: 60 }}>Loading...</div>;

  if (!employee) {
    return (
      <div style={{ textAlign: "center", marginTop: 60 }}>
        <h2>Unauthorized</h2>
        <p>Please log in to view your dashboard.</p>
      </div>
    );
  }

  // Format timer as HH:MM:SS
  const formatTimer = (t) => {
    const h = String(Math.floor(t / 3600)).padStart(2, '0');
    const m = String(Math.floor((t % 3600) / 60)).padStart(2, '0');
    const s = String(t % 60).padStart(2, '0');
    return `${h}:${m}:${s}`;
  };

  return (
    <div style={{ minHeight: "100vh", background: "#f8fafc", padding: 0 }}>
      {/* Top Bar */}
      <div style={{
        width: "100%",
        background: "#3b82f6",
        color: "#fff",
        padding: "18px 0 18px 32px",
        fontWeight: 600,
        fontSize: "1.2em",
        letterSpacing: "1px",
        boxShadow: "0 2px 8px #b6b6b6"
      }}>
        Welcome, {employee.username || employee.name}
      </div>
      {/* Main Content */}
      <div style={{ maxWidth: 600, margin: "40px auto", fontFamily: "sans-serif", background: "#fff", borderRadius: 12, boxShadow: "0 4px 24px #b6b6b6", padding: 32 }}>
        <div style={{ marginBottom: 24 }}>
          <h4 style={{ color: "#6366f1" }}>About You</h4>
          <p><b>Name:</b> {employee.name}</p>
          <p><b>Email:</b> {employee.email}</p>
          <p><b>Employee ID:</b> {employee.id}</p>
        </div>
        <div style={{ marginBottom: 24 }}>
          <h4 style={{ color: "#6366f1" }}>Company</h4>
          <p>Acme Corporation Pvt. Ltd.</p>
          <p>Department: Engineering</p>
          <p>Location: Remote</p>
        </div>
        <div>
          <h4 style={{ color: "#6366f1" }}>Your Work</h4>
          <ul>
            <li>Project: Punch API System</li>
            <li>Status: Active</li>
            <li>Last Punch: (show punch info here if needed)</li>
          </ul>
        </div>
        {/* Timer and Punch Out */}
        <div style={{ marginTop: 32, textAlign: "center" }}>
          <div style={{ fontSize: "2em", fontWeight: 700, color: "#3b82f6", marginBottom: 16 }}>{formatTimer(timer)}</div>
          <button onClick={handlePunchOut} disabled={!timerActive} style={{
            background: timerActive ? "#ef4444" : "#d1d5db",
            color: timerActive ? "#fff" : "#888",
            border: "none",
            borderRadius: 8,
            padding: "14px 36px",
            fontWeight: 600,
            fontSize: "1.1em",
            cursor: timerActive ? "pointer" : "not-allowed",
            boxShadow: "0 2px 8px #d1d5db"
          }}>Punch Out</button>
          {workMessage && <div style={{ marginTop: 24, color: "#065f46", fontWeight: 600, fontSize: "1.1em" }}>{workMessage}</div>}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;

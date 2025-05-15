import React, { useState } from "react";
import { useNavigate, Link, useLocation } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import "./auth.css";

const SignupForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [businessName, setBusinessName] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { signUp } = useAuth();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);


  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setError("Passwords do not match!");
      return;
    }
    setError("");
    setIsLoading(true);

    try {
      const response = await signUp(
        email,
        password,
        confirmPassword,
        businessName
      );
      if (response) {
        const from = (location.state as any)?.from?.pathname || "/dashboard";
        navigate(from, { replace: true });
      } else {
        setError("Invalid email or password");
      }
    } catch (err) {
      console.error("Signup error:", err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2 className="auth-title">Create Account</h2>
        <p className="auth-subtitle">Please fill in your details</p>
        {error && <div className="alert alert-danger">{error}</div>}
        <form onSubmit={handleSubmit}>
        <div className="form-floating mb-3">
            <input
              type="email"
              id="email"
              className="form-control"
              placeholder="name@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              disabled={isLoading}
            />
            <label htmlFor="email">Email address</label>
          </div>
          <div className="form-floating mb-3">
            <input
              type="text"
              id="businessName"
              className="form-control"
              placeholder="Institution Name"
              value={businessName}
              onChange={(e) => setBusinessName(e.target.value)}
              disabled={isLoading}
            />
            <label htmlFor="businessName">Business Name</label>
          </div>
          <div className="form-floating mb-3 position-relative">
            <input
              type={showPassword ? "text" : "password"}
              id="password"
              className="form-control"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              disabled={isLoading}
            />
            <label htmlFor="password">Password</label>
            <span
              className="position-absolute top-50 end-0 translate-middle-y me-2"
              style={{ cursor: "pointer" }}
              onClick={() => setShowPassword((prev) => !prev)}
            >
              <i
                 className={`bx ${showPassword ? "bx-hide" : "bx-show"}`}
                 style={{ fontSize: "1.2rem" }}
              ></i>{" "}
            </span>
          </div>
          <div className="form-floating mb-3 position-relative">
            <input
              type={showConfirmPassword ? "text" : "password"}
              id="confirmPassword"
              className="form-control"
              placeholder="Confirm Password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              disabled={isLoading}
            />
            <label htmlFor="confirmPassword">Confirm Password</label>
            <span
              className="position-absolute top-50 end-0 translate-middle-y me-2"
              style={{ cursor: "pointer" }}
              onClick={() => setShowConfirmPassword((prev) => !prev)}
            >
              <i
                 className={`bx ${showConfirmPassword ? "bx-hide" : "bx-show"}`}
                 style={{ fontSize: "1.2rem" }}
              ></i>{" "}
            </span>
          </div>
          <button
            type="submit"
            className="btn btn-primary w-100 d-flex align-items-center justify-content-center gap-2"
            disabled={isLoading}
          >
            {isLoading ? (
              <>
                <span
                  className="spinner-border spinner-border-sm"
                  role="status"
                  aria-hidden="true"
                ></span>
                <span>Signing up...</span>
              </>
            ) : (
              <>
                <i className="bx bx-log-in"></i>
                <span>Sign Up</span>
              </>
            )}
          </button>
          <div className="text-center mt-3">
            <span>Already have an account? </span>
            <Link to="/login">Log In</Link>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SignupForm;

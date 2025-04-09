import { useState } from 'react';
import '../styles/LoginRegister.css';
import { login, register } from '../services/authService';

export default function LoginRegister() {
  const [isFlipped, setIsFlipped] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [formErrors, setFormErrors] = useState({});

  // Password validation
  const validatePassword = (pass) => {
    const errors = {};
    if (pass.length < 10) errors.length = true;
    if (!/[A-Z]/.test(pass)) errors.uppercase = true;
    if (!/[0-9]/.test(pass)) errors.number = true;
    if (!/[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(pass)) errors.special = true;
    return errors;
  };

  const handlePasswordChange = (e) => {
    const newPassword = e.target.value;
    setPassword(newPassword);
    if (isFlipped) {
      const passwordErrors = validatePassword(newPassword);
      setFormErrors({ ...formErrors, password: passwordErrors });
    }
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    console.log('Logging in with:', { email, password });
    try {
      const userData = await login({ email, password });
      console.log('Login successful:', userData);
      // Here you would typically:
      // 1. Store the user data or token in local storage or context
      // 2. Redirect to a dashboard or home page
      // 3. Update your app's authentication state
    } catch (error) {
      console.error('Login failed:', error);
      // Here you would typically show an error message to the user
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const userData = await register({ name, email, password });
      console.log('Register successful:', userData);
    } catch (error) {
      console.error('Register failed:', error);
    }
  };

  const toggleFlip = () => {
    setIsFlipped(!isFlipped);
    setFormErrors({});
    setPassword('');
  };

  const passwordHasErrors = formErrors.password && Object.keys(formErrors.password).length > 0;

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="form-container">
          <div className={`login-form ${isFlipped ? 'hidden' : 'visible'}`}>
            <div className="card">
              <h2>Welcome Back</h2>

              <form onSubmit={handleLogin}>
                <div className="form-group">
                  <label htmlFor="email">Email</label>
                  <div className="input-container">
                    <span className="input-icon">✉️</span>
                    <input
                      id="email"
                      type="email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      className="input-field"
                      placeholder="your@email.com"
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label htmlFor="password">Password</label>
                  <div className="input-container">
                    <span className="input-icon">🔒</span>
                    <input
                      id="password"
                      type={showPassword ? "text" : "password"}
                      value={password}
                      onChange={handlePasswordChange}
                      className="input-field"
                      placeholder="Your password"
                      required
                    />
                    <button
                      type="button"
                      className="password-toggle"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? "👁️" : "👁️‍🗨️"}
                    </button>
                  </div>
                </div>

                <button
                  type="submit"
                  className="btn btn-primary"
                >
                  Log In
                </button>
              </form>

              <div className="form-footer">
                <p>
                  Don't have an account?{" "}
                  <button
                    onClick={toggleFlip}
                    className="link"
                  >
                    Register
                  </button>
                </p>
              </div>
            </div>
          </div>

          <div className={`register-form ${isFlipped ? 'visible' : 'hidden'}`}>
            <div className="card">
              <h2>Create Account</h2>

              <form onSubmit={handleRegister}>
                <div className="form-group">
                  <label htmlFor="name">Full Name</label>
                  <div className="input-container">
                    <span className="input-icon">👤</span>
                    <input
                      id="name"
                      type="text"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                      className="input-field"
                      placeholder="John Doe"
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label htmlFor="register-email">Email</label>
                  <div className="input-container">
                    <span className="input-icon">✉️</span>
                    <input
                      id="register-email"
                      type="email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      className="input-field"
                      placeholder="your@email.com"
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label htmlFor="register-password">Password</label>
                  <div className="input-container">
                    <span className="input-icon">🔒</span>
                    <input
                      id="register-password"
                      type={showPassword ? "text" : "password"}
                      value={password}
                      onChange={handlePasswordChange}
                      className={`input-field ${passwordHasErrors ? 'error' : ''}`}
                      placeholder="Create a password"
                      required
                    />
                    <button
                      type="button"
                      className="password-toggle"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? "👁️" : "👁️‍🗨️"}
                    </button>
                  </div>
                </div>

                <div className="password-requirements">
                  <p>Password requirements:</p>
                  <ul className="requirements-list">
                    <li className={`requirement-item ${password.length >= 10 ? 'requirement-valid' :
                      formErrors.password?.length ? 'requirement-invalid' : 'requirement-neutral'
                      }`}>
                      <span className="requirement-icon">
                        {password.length >= 10 ? "✓" : "✗"}
                      </span>
                      At least 10 characters
                    </li>
                    <li className={`requirement-item ${/[A-Z]/.test(password) ? 'requirement-valid' :
                      formErrors.password?.uppercase ? 'requirement-invalid' : 'requirement-neutral'
                      }`}>
                      <span className="requirement-icon">
                        {/[A-Z]/.test(password) ? "✓" : "✗"}
                      </span>
                      One uppercase letter
                    </li>
                    <li className={`requirement-item ${/[0-9]/.test(password) ? 'requirement-valid' :
                      formErrors.password?.number ? 'requirement-invalid' : 'requirement-neutral'
                      }`}>
                      <span className="requirement-icon">
                        {/[0-9]/.test(password) ? "✓" : "✗"}
                      </span>
                      One number
                    </li>
                    <li className={`requirement-item ${/[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(password) ? 'requirement-valid' :
                      formErrors.password?.special ? 'requirement-invalid' : 'requirement-neutral'
                      }`}>
                      <span className="requirement-icon">
                        {/[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(password) ? "✓" : "✗"}
                      </span>
                      One special character
                    </li>
                  </ul>
                </div>

                <button
                  type="submit"
                  className="btn btn-primary"
                  disabled={passwordHasErrors}
                  onClick={handleRegister}
                >
                  Register
                </button>
              </form>

              <div className="form-footer">
                <p>
                  Already have an account?{" "}
                  <button
                    onClick={toggleFlip}
                    className="link"
                  >
                    Log In
                  </button>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
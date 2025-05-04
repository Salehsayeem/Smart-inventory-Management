// import React, { useState } from 'react';
// import { apiService } from '../../api/apiService';
// import { useAuth } from '../../context/AuthContext';
// import { useLoader } from '../../context/LoaderContext';
// import { Form, Button, Container, Row, Col, Card } from 'react-bootstrap';
// import { showError } from '../../utils/toastService';
// import { useNavigate, Link } from 'react-router-dom';

// const SignupForm: React.FC = () => {
//   const { login } = useAuth();
//   const { setLoading } = useLoader();
//   const [email, setEmail] = useState('');
//   const [password, setPassword] = useState('');
//   const [confirmPassword, setConfirmPassword] = useState('');
//   const [isLoading, setIsLoading] = useState(false);
//   const navigate = useNavigate();

//   const handleSignup = async (e: React.FormEvent) => {
//     e.preventDefault();
//     if (password !== confirmPassword) {
//       showError('Passwords do not match!');
//       return;
//     }
//     setIsLoading(true);
//     setLoading(true);
//     try {
//       const response = await apiService.post<{ token: string }>('/auth/register', { email, password });
//       login(response.token); // Authenticate user immediately
//       navigate('/dashboard');
//     } catch (err) {
//       console.error('Signup error:', err);
//     } finally {
//       setIsLoading(false);
//       setLoading(false);
//     }
//   };

//   return (
//     <Container fluid className="d-flex align-items-center justify-content-center min-vh-100 bg-light">
//       <Row className="w-100">
//         <Col md={6} lg={4} className="mx-auto">
//           <Card className="shadow-sm p-4">
//             <Card.Body>
//               {isLoading ? (
//                 <div className="d-flex justify-content-center">
//                   <div className="spinner-border text-primary" role="status">
//                     <span className="visually-hidden">Loading...</span>
//                   </div>
//                 </div>
//               ) : (
//                 <>
//                   <h2 className="text-center mb-4">Create Account</h2>
//                   <Form onSubmit={handleSignup}>
//                     <Form.Group className="mb-3" controlId="signupEmail">
//                       <Form.Control
//                         type="email"
//                         placeholder="Your email address"
//                         value={email}
//                         onChange={(e) => setEmail(e.target.value)}
//                         required
//                       />
//                     </Form.Group>

//                     <Form.Group className="mb-3" controlId="signupPassword">
//                       <Form.Control
//                         type="password"
//                         placeholder="Enter password"
//                         value={password}
//                         onChange={(e) => setPassword(e.target.value)}
//                         required
//                       />
//                     </Form.Group>

//                     <Form.Group className="mb-3" controlId="confirmPassword">
//                       <Form.Control
//                         type="password"
//                         placeholder="Confirm password"
//                         value={confirmPassword}
//                         onChange={(e) => setConfirmPassword(e.target.value)}
//                         required
//                       />
//                     </Form.Group>

//                     <Button type="submit" className="w-100">
//                       Sign Up
//                     </Button>

//                     <div className="text-center mt-3">
//                       <span>Already have an account? </span>
//                       <Link to="/login">Log In</Link>
//                     </div>
//                   </Form>
//                 </>
//               )}
//             </Card.Body>
//           </Card>
//         </Col>
//       </Row>
//     </Container>
//   );
// };

// export default SignupForm;
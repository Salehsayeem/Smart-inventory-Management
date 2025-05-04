import React, { useState } from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import { motion } from 'framer-motion';
import { NavLink, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import './Sidebar.css';

const menuItems = [
  { icon: 'home', label: 'Dashboard', link: '/dashboard' },
  { icon: 'pie-chart-alt', label: 'Analytics', link: '/analytics' },
  { icon: 'time', label: 'Schedules', link: '/schedules' },
  { icon: 'user', label: 'Acount Manager', link: '/account-manager' },
  { icon: 'folder', label: 'File Manager', link: '/file-manager' },
  { icon: 'group', label: 'Group Manager', link: '/group-manager' },
];

const logoutItem = { icon: 'log-out', label: 'Logout', link: '/logout' };

const Sidebar: React.FC = () => {
  const [isOpen, setIsOpen] = useState(true);
  const location = useLocation();
  const navigate = useNavigate();
  const { logout } = useAuth();
  const activeItem =
    menuItems.find((item) => location.pathname === item.link)?.label ||
    (location.pathname === logoutItem.link ? logoutItem.label : 'Dashboard');

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <>
      <motion.div
        className={`sidebar-custom d-none d-md-flex flex-column align-items-center ${isOpen ? 'expanded' : 'collapsed'}`}
        initial={false}
        animate={{ width: isOpen ? 220 : 80 }}
        transition={{ duration: 0.2 }}
      >
        {/* Top Part: Avatar, Username, Email */}
        <div className="sidebar-top-part w-100 d-flex flex-column align-items-center pt-4 pb-3">
          <img
            src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
            width={56}
            height={56}
            alt="User"
            className="rounded-circle sidebar-avatar mb-2"
          />
          {isOpen && (
            <>
              <div className="sidebar-username fw-bold">John Doe</div>
              <div className="sidebar-email text-muted small">john.doe@example.com</div>
            </>
          )}
        </div>

        {/* Middle Part: Menu Items + Logout */}
        <div className="sidebar-middle-part flex-grow-1 w-100 d-flex flex-column align-items-center">
          <div className="w-100 flex-grow-1 d-flex flex-column align-items-center">
            {menuItems.map((item) => (
              <NavLink
                to={item.link}
                key={item.label}
                className={({ isActive }) =>
                  `sidebar-item-custom my-1 ${isActive ? 'active' : ''} ${isOpen ? 'expanded' : 'collapsed'}`
                }
                style={{ textDecoration: 'none' }}
              >
                <div className="sidebar-icon-wrap d-flex align-items-center justify-content-center">
                  <i
                    className={`bx ${activeItem === item.label ? 'bxs' : 'bx'}-${item.icon}`}
                    style={{ fontSize: '2rem', color: activeItem === item.label ? '#4f6bed' : '#7a7f9a' }}
                  ></i>
                </div>
                {isOpen && (
                  <motion.span
                    className="sidebar-label-custom ms-3"
                    initial={{ opacity: 0, x: 10 }}
                    animate={{ opacity: 1, x: 0 }}
                    exit={{ opacity: 0, x: 10 }}
                    transition={{ duration: 0.2 }}
                  >
                    {item.label}
                  </motion.span>
                )}
              </NavLink>
            ))}
          </div>
          {/* Logout Option */}
          <button
            onClick={handleLogout}
            className={`sidebar-item-custom my-1 sidebar-logout ${isOpen ? 'expanded' : 'collapsed'}`}
            style={{ textDecoration: 'none', border: 'none', background: 'none', width: '100%' }}
          >
            <div className="sidebar-icon-wrap d-flex align-items-center justify-content-center">
              <i
                className={`bx ${activeItem === logoutItem.label ? 'bxs' : 'bx'}-${logoutItem.icon}`}
                style={{ fontSize: '2rem', color: activeItem === logoutItem.label ? '#4f6bed' : '#7a7f9a' }}
              ></i>
            </div>
            {isOpen && (
              <motion.span
                className="sidebar-label-custom ms-3"
                initial={{ opacity: 0, x: 10 }}
                animate={{ opacity: 1, x: 0 }}
                exit={{ opacity: 0, x: 10 }}
                transition={{ duration: 0.2 }}
              >
                {logoutItem.label}
              </motion.span>
            )}
          </button>
        </div>

        {/* Bottom Part: Collapsible Icon */}
        <div className="sidebar-bottom-part mb-4">
          <button
            className="sidebar-toggle-btn-custom d-flex align-items-center justify-content-center"
            onClick={() => setIsOpen((prev) => !prev)}
          >
            <i className={`bx bx-chevron-${isOpen ? 'left' : 'right'}`} style={{ color: '#7a7f9a', fontSize: '1.7rem' }}></i>
          </button>
        </div>
      </motion.div>

      {/* Mobile Bottom Navigation Bar */}
      <Navbar fixed="bottom" className="d-md-none bg-light border-top p-0">
        <Nav className="w-100 d-flex flex-row justify-content-around">
          {[...menuItems, logoutItem].map((item) => (
            <NavLink
              to={item.link}
              key={item.label}
              className={({ isActive }) =>
                `text-center py-2 flex-fill ${isActive ? 'bg-primary text-white' : ''}`
              }
              style={{ borderRadius: 12, margin: '0 2px', textDecoration: 'none' }}
              onClick={item.label === 'Logout' ? handleLogout : undefined}
            >
              <i
                className={`bx ${activeItem === item.label ? 'bxs' : 'bx'}-${item.icon}`}
                style={{ fontSize: '1.7rem', color: activeItem === item.label ? '#fff' : '#7a7f9a' }}
              ></i>
              <div style={{ fontSize: '0.75rem' }}>{item.label}</div>
            </NavLink>
          ))}
        </Nav>
      </Navbar>
    </>
  );
};

export default Sidebar; 
import React, { useState, useEffect } from "react";
import { Navbar, Nav } from "react-bootstrap";
import { motion, AnimatePresence } from "framer-motion";
import { NavLink, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { showSuccess } from "../../utils/toastService";
import { PermissionDetails } from "../../types";
import { getPermissionsFromCookie } from "../../utils/cookieUtils";
import "./Sidebar.css";

const logoutItem = {
  id: "logout",
  icon: "log-out",
  label: "Logout",
  path: "/logout",
};

const shops = [
  { id: 1, label: "Shop 1", subLabel: "New York" },
  { id: 2, label: "Shop 2", subLabel: "Los Angeles" },
  { id: 3, label: "Shop 3", subLabel: "Chicago" },
  { id: 4, label: "Shop 4", subLabel: "Miami" },
];

const Sidebar: React.FC = () => {
  const [isOpen, setIsOpen] = useState(true);
  const [selectedShop, setSelectedShop] = useState(shops[0]);
  const [menuItems, setMenuItems] = useState<PermissionDetails[]>([]);
  const [isShopDropdownOpen, setIsShopDropdownOpen] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();
  const { logout, userInfo } = useAuth();

  useEffect(() => {
    const permissions = getPermissionsFromCookie();
    let PermissionDetails: PermissionDetails[] = [];
    if (typeof permissions === "string") {
      try {
        PermissionDetails = JSON.parse(permissions).PermissionDetails;
        if (PermissionDetails.length > 0) {
          setMenuItems(PermissionDetails);
        }
      } catch (error) {
        console.error("Error parsing permissions:", error);
      }
    }
  }, []);

  const activeItem =
    menuItems.find(
      (item: PermissionDetails) => location.pathname === `/${item.Path}`
    ) ||
    (location.pathname === logoutItem.path ? logoutItem.label : "Dashboard");

  const handleLogout = () => {
    logout();
    showSuccess("Logged out successfully");
    navigate("/login");
  };

  const handleShopSelect = (shopId: number) => {
    const shop = shops.find((s) => s.id === shopId);
    if (shop) {
      setSelectedShop(shop);
      setIsShopDropdownOpen(false);
    }
  };

  return (
    <>
      <motion.div
        className={`sidebar-custom d-none d-md-flex flex-column align-items-center ${
          isOpen ? "expanded" : "collapsed"
        }`}
        initial={false}
        animate={{ width: isOpen ? 280 : 80 }}
        transition={{ duration: 0.3, ease: "easeInOut" }}
      >
        {/* Top Part: Avatar, Username, Email */}
        <div className="sidebar-top-part w-100 d-flex flex-column align-items-center pt-4 pb-3">
          <motion.div
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            transition={{ duration: 0.3 }}
          >
            <img
              src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
              width={64}
              height={64}
              alt="User"
              className="rounded-circle sidebar-avatar mb-2"
            />
          </motion.div>
          <AnimatePresence>
            {isOpen && (
              <motion.div
                initial={{ opacity: 0, y: -10 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -10 }}
                transition={{ duration: 0.2 }}
                className="text-center"
              >
                <div className="sidebar-username">
                  {userInfo?.name || "User"}
                </div>
                <div className="sidebar-email">
                  {userInfo?.email || "user@example.com"}
                </div>
                <div className="sidebar-role">{userInfo?.role || "User"}</div>
                <div className="sidebar-shop-container">
                  <select
                    value={selectedShop.id}
                    onChange={(e) => handleShopSelect(Number(e.target.value)) }
                  >
                    {shops.map((shop) => (
                      <option key={`shop-${shop.id}`} value={shop.id}>
                        {shop.label}
                      </option>
                    ))}
                  </select>
                </div>
                {/* <div className="sidebar-shop-container">
                  <div 
                    className="dropdown-container"
                    onMouseEnter={() => setIsShopDropdownOpen(true)}
                    onMouseLeave={() => setIsShopDropdownOpen(false)}
                  >
                    <div className="dropdown-trigger">
                      <i className="bx bx-store"></i>
                      <span className="dropdown-label">{selectedShop.label}</span>
                      <i className={`bx bx-chevron-${isShopDropdownOpen ? 'up' : 'down'} dropdown-chevron`}></i>
                    </div>
                    <AnimatePresence>
                      {isShopDropdownOpen && (
                        <motion.div
                          initial={{ opacity: 0, y: -10 }}
                          animate={{ opacity: 1, y: 0 }}
                          exit={{ opacity: 0, y: -10 }}
                          transition={{ duration: 0.2 }}
                          className="mobile-shop-select"
                        >
                          {shops.map((shop) => (
                            <div
                              key={`shop-${shop.id}`}
                              className={`shop-option ${selectedShop.id === shop.id ? 'selected' : ''}`}
                              onClick={() => handleShopSelect(shop.id)}
                            >
                              {shop.label}
                            </div>
                          ))}
                        </motion.div>
                      )}
                    </AnimatePresence>
                  </div>
                </div> */}
              </motion.div>
            )}
          </AnimatePresence>
        </div>

        {/* Middle Part: Menu Items + Logout */}
        <div className="sidebar-middle-part flex-grow-1 w-100 d-flex flex-column align-items-center">
          <div className="w-100 flex-grow-1 d-flex flex-column align-items-center">
            {menuItems.map((item: PermissionDetails) => (
              <NavLink
                to={`/${item.Path}`}
                key={item.Id}
                className={({ isActive }) =>
                  `sidebar-item-custom my-1 ${isActive ? "active" : ""} ${
                    isOpen ? "expanded" : "collapsed"
                  }`
                }
                style={{ textDecoration: "none" }}
              >
                <div className="sidebar-icon-wrap">
                  <i
                    className={`bx ${
                      activeItem === item.ModuleName ? "bxs" : "bx"
                    }-${item.MenuIcon}`}
                  ></i>
                </div>
                <AnimatePresence>
                  {isOpen && (
                    <motion.span
                      className="sidebar-label-custom ms-3"
                      initial={{ opacity: 0, x: -10 }}
                      animate={{ opacity: 1, x: 0 }}
                      exit={{ opacity: 0, x: -10 }}
                      transition={{ duration: 0.2 }}
                    >
                      {item.ModuleName}
                    </motion.span>
                  )}
                </AnimatePresence>
              </NavLink>
            ))}
          </div>
          {/* Logout Option */}
          <button
            onClick={handleLogout}
            className={`sidebar-item-custom my-1 sidebar-logout ${
              isOpen ? "expanded" : "collapsed"
            }`}
            style={{
              textDecoration: "none",
              border: "none",
              background: "none",
              width: "100%",
            }}
          >
            <div className="sidebar-icon-wrap">
              <i
                className={`bx ${
                  activeItem === logoutItem.label ? "bxs" : "bx"
                }-${logoutItem.icon}`}
              ></i>
            </div>
            <AnimatePresence>
              {isOpen && (
                <motion.span
                  className="sidebar-label-custom ms-3"
                  initial={{ opacity: 0, x: -10 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0, x: -10 }}
                  transition={{ duration: 0.2 }}
                >
                  {logoutItem.label}
                </motion.span>
              )}
            </AnimatePresence>
          </button>
        </div>

        {/* Bottom Part: Collapsible Icon */}
        <div className="sidebar-bottom-part">
          <button
            className={`sidebar-toggle-btn-custom ${
              !isOpen ? "collapsed" : ""
            }`}
            onClick={() => setIsOpen((prev) => !prev)}
          >
            <i className={`bx bx-chevron-${isOpen ? "left" : "right"}`}></i>
          </button>
        </div>
      </motion.div>

      {/* Mobile Shop Dropdown */}
      <div className="d-md-none mobile-shop-dropdown">
        <select
          value={selectedShop.id}
          onChange={(e) => handleShopSelect(Number(e.target.value))}
        >
          {shops.map((shop) => (
            <option key={`shop-${shop.id}`} value={shop.id}>
              {shop.label}
            </option>
          ))}
        </select>
      </div>

      {/* Mobile Bottom Navigation Bar */}
      <Navbar fixed="bottom" className="d-md-none bg-light border-top p-0">
        <Nav className="w-100 d-flex flex-row justify-content-around">
          {menuItems.map((item: PermissionDetails) => (
            <NavLink
              to={`/${item.Path}`}
              key={item.Id}
              className={({ isActive }) =>
                `text-center py-2 flex-fill ${isActive ? "active" : ""}`
              }
            >
              <i
                className={`bx ${
                  activeItem === item.ModuleName ? "bxs" : "bx"
                }-${item.MenuIcon}`}
              ></i>
            </NavLink>
          ))}
          {/* Logout Button */}
          <NavLink
            to={logoutItem.path}
            key={logoutItem.id}
            className={({ isActive }) =>
              `text-center py-2 flex-fill ${isActive ? "active" : ""}`
            }
            onClick={handleLogout}
          >
            <i
              className={`bx ${
                activeItem === logoutItem.label ? "bxs" : "bx"
              }-${logoutItem.icon}`}
            ></i>
          </NavLink>
        </Nav>
      </Navbar>
    </>
  );
};

export default Sidebar;

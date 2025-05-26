import React, { useState, useEffect } from "react";
import { Navbar, Nav } from "react-bootstrap";
import { motion, AnimatePresence } from "framer-motion";
import { NavLink, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { showSuccess } from "../../utils/toastService";
import { PermissionDetails, RegisteredShops } from "../../types";
import {
  getPermissionsFromCookie,
  setSelectedShopInCookie,
} from "../../utils/cookieUtils";
import "./Sidebar.css";
import { keysToCamel } from "../../utils/caseUtils";

const logoutItem = {
  id: "logout",
  icon: "log-out",
  label: "Logout",
  path: "/logout",
};

const Sidebar: React.FC = () => {
  const [isOpen, setIsOpen] = useState(true);
  const [selectedShop, setSelectedShop] = useState<RegisteredShops | null>(
    null
  );
  const [menuItems, setMenuItems] = useState<PermissionDetails[]>([]);
  const [shops, setShops] = useState<RegisteredShops[]>([]);
  const [, setIsShopDropdownOpen] = useState(false);
  const [isMoreDropdownOpen, setIsMoreDropdownOpen] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();
  const { logout, userInfo } = useAuth();
  useEffect(() => {
    const permissions = getPermissionsFromCookie();
    let permissionDetails: PermissionDetails[] = [];
    if (typeof permissions === "string") {
      try {
        permissionDetails = JSON.parse(permissions).PermissionDetails;
        permissionDetails = keysToCamel(permissionDetails);
        if (permissionDetails.length > 0) {
          setMenuItems(permissionDetails);

        }
      } catch (error) {
        console.error("Error parsing permissions:", error);
      }
    }
    try {
      if (userInfo?.shops) {

        let parsedShops =
          typeof userInfo.shops === "string"
            ? JSON.parse(userInfo.shops)
            : userInfo.shops;
        if (Array.isArray(parsedShops)) {
          parsedShops = keysToCamel(parsedShops);
          setShops(parsedShops);
          if (parsedShops.length > 0) {
            setSelectedShop(parsedShops[0]);
            setSelectedShopInCookie(parsedShops[0].Id);
          }
        } else {
          console.warn("Parsed shops is not a valid array:", parsedShops);
        }
      }
    } catch (error) {
      console.error("Error parsing shops:", error);
    }
  }, [userInfo]);

  const activeItem =
    menuItems.find(
      (item: PermissionDetails) => location.pathname === `/${item.path}`
    ) ||
    (location.pathname === logoutItem.path ? logoutItem.label : "Dashboard");

  const handleLogout = () => {
    logout();
    showSuccess("Logged out successfully");
    navigate("/login");
  };

  const handleShopSelect = (shopId: number) => {
    const shop = userInfo?.shops.find((s) => s.id === shopId);
    if (shop) {
      setSelectedShop(shop);
      setIsShopDropdownOpen(false);
      setSelectedShopInCookie(userInfo!.shops[0].id);
    }
  };

  const visibleMenuItemsMobile = menuItems.slice(0, 2);
  const dropdownMenuItemsMobile = menuItems.slice(2);
  return (
    <>
      <motion.div
        className={`sidebar-custom d-none d-md-flex flex-column align-items-center ${isOpen ? "expanded" : "collapsed"
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
                    value={selectedShop?.id || "0"}
                    onChange={(e) => handleShopSelect(Number(e.target.value))}
                  >
                    {Array.isArray(shops)
                      ? shops.map((shop) => (
                        <option key={`shop-${shop.id}`} value={shop.id}>
                          {shop.name}
                        </option>
                      ))
                      : null}
                  </select>
                </div>
              </motion.div>
            )}
          </AnimatePresence>
        </div>

        {/* Middle Part: Menu Items + Logout */}
        <div className="sidebar-middle-part flex-grow-1 w-100 d-flex flex-column align-items-center">
          <div className="w-100 flex-grow-1 d-flex flex-column align-items-center">
            {menuItems.map((item: PermissionDetails) => (
              <NavLink
                to={`/${item.path}`}
                key={item.id}
                className={({ isActive }) =>
                  `sidebar-item-custom my-1 ${isActive ? "active" : ""} ${isOpen ? "expanded" : "collapsed"
                  }`
                }
                style={{ textDecoration: "none" }}
              >
                <div className="sidebar-icon-wrap">
                  <i
                    className={`bx ${activeItem === item.moduleName ? "bxs" : "bx"
                      }-${item.menuIcon}`}
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
                      {item.moduleName}
                    </motion.span>
                  )}
                </AnimatePresence>
              </NavLink>
            ))}
          </div>
          {/* Logout Option */}
          <button
            onClick={handleLogout}
            className={`sidebar-item-custom my-1 sidebar-logout ${isOpen ? "expanded" : "collapsed"
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
                className={`bx ${activeItem === logoutItem.label ? "bxs" : "bx"
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
            className={`sidebar-toggle-btn-custom ${!isOpen ? "collapsed" : ""
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
          value={selectedShop?.id || "0"}
          onChange={(e) => handleShopSelect(Number(e.target.value))}
        >
          {Array.isArray(shops)
            ? shops.map((shop) => (
              <option key={`shop-${shop.id}`} value={shop.id}>
                {shop.name}
              </option>
            ))
            : null}
        </select>
      </div>

      {/* Mobile Bottom Navigation Bar */}
      <Navbar
        fixed="bottom"
        className="d-md-none bg-light border-top p-0 mobile-bottom-navbar"
      >
        <Nav className="w-100 d-flex flex-row justify-content-around align-items-center">
          {visibleMenuItemsMobile.map((item: PermissionDetails) => (
            <NavLink
              to={`/${item.path}`}
              key={`mobile-${item.id}`}
              className={({ isActive }) =>
                `text-center py-2 flex-fill mobile-nav-item ${isActive ? "active" : ""
                }`
              }
            >
              <i
                className={`bx ${activeItem === item.moduleName ? "bxs" : "bx"
                  }-${item.menuIcon}`}
              ></i>
            </NavLink>
          ))}

          {/* More Items Dropdown Icon (if more than 2 items) */}
          {dropdownMenuItemsMobile.length > 0 && (
            <div
              className="text-center py-2 flex-fill mobile-nav-item more-dropdown-container"
              onMouseEnter={() => setIsMoreDropdownOpen(true)}
              onMouseLeave={() => setIsMoreDropdownOpen(false)}
            >
              <i className="bx bx-menu"></i> {/* Bar icon for more items */}
              {/* <span className="mobile-nav-label">More</span> */}
              <AnimatePresence>
                {isMoreDropdownOpen && (
                  <motion.div
                    className="more-dropdown-menu"
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: 10 }}
                    transition={{ duration: 0.2 }}
                  >
                    {dropdownMenuItemsMobile.map((item: PermissionDetails) => (
                      <NavLink
                        to={`/${item.path}`}
                        key={`mobile-dropdown-${item.id}`}
                        className={({ isActive }) =>
                          `dropdown-item-custom ${isActive ? "active" : ""}`
                        }
                        onClick={() => setIsMoreDropdownOpen(false)} // Close dropdown on click
                      >
                        <i
                          className={`bx ${activeItem === item.moduleName ? "bxs" : "bx"
                            }-${item.menuIcon}`}
                        ></i>
                        <span>{item.moduleName}</span>
                      </NavLink>
                    ))}
                  </motion.div>
                )}
              </AnimatePresence>
            </div>
          )}

          {/* If there are less than 2 menu items and no dropdown, add a placeholder or adjust flex for the remaining items + logout */}
          {/* This ensures 4 slots are conceptually filled or spaced correctly */}
          {menuItems.length < 2 && dropdownMenuItemsMobile.length === 0 && (
            <div className="text-center py-2 flex-fill mobile-nav-item"></div>
          )}
          {menuItems.length < 1 && dropdownMenuItemsMobile.length === 0 && (
            <div className="text-center py-2 flex-fill mobile-nav-item"></div>
          )}

          {/* Logout Button */}
          <NavLink
            to={logoutItem.path}
            key={`mobile-${logoutItem.id}`}
            className={({ isActive }) =>
              `text-center py-2 flex-fill mobile-nav-item ${isActive ? "active" : ""
              }`
            }
            onClick={handleLogout}
          >
            <i
              className={`bx ${activeItem === logoutItem.label ? "bxs" : "bx"
                }-${logoutItem.icon}`}
            ></i>
          </NavLink>
        </Nav>
      </Navbar>
    </>
  );
};

export default Sidebar;

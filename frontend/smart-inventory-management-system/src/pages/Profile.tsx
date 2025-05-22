import React, { useEffect, useState } from "react";
import "./Profile.css";
import { getUserProfile, updateUserProfile } from "../api/apiService";
import { getUserIdFromToken } from "../utils/cookieUtils";
import { ProfileType, RegisteredShops } from "../types";
import EditableField from "../components/editable-input-field/EditableField";
import GenericModal from "../components/modal/Modal";

const Profile: React.FC = () => {
  const [profile, setProfile] = useState<ProfileType | null>(null);
  const [shops, setShops] = useState<RegisteredShops[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [fullName, setFullName] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    fetchProfile();
  }, []);
  const fetchProfile = async () => {
    try {
      const userId = getUserIdFromToken();
      if (userId) {
        const userProfile = await getUserProfile(userId);
        const mapUserShop = Array.isArray(userProfile.registeredShops)
          ? userProfile.registeredShops.map((shop: any) => ({
              id: shop.id,
              name: shop.name,
              address: shop.address,
            }))
          : [];
        setShops(mapUserShop);
        setProfile(userProfile);
        setFullName(userProfile.fullName);
      }
    } catch (err) {
      console.error("Failed to fetch profile", err);
    }
  };
  const handleSave = async () => {
    if (!profile) return;
    try {
      await updateUserProfile({
        id: profile.id,
        fullName,
      });
      fetchProfile();
      setIsEditing(false);
    } catch (err) {
      console.error("Update failed", err);
    }
  };
  return (
    <div className="dashboard-container">
      <h1>Profile</h1>
      <div className="dashboard-grid">
        <div className="dashboard-card">
          <div className="card-header">
            <h3>
              Personal Information of <em>{profile?.roleName}</em>
            </h3>
            <div className="edit-actions">
              {isEditing ? (
                <>
                  <button className="view-all-btn" onClick={handleSave}>
                    Save
                  </button>
                  <button
                    className="cancel-btn"
                    onClick={() => setIsEditing(false)}
                    title="Cancel"
                  >
                    ✖
                  </button>
                </>
              ) : (
                <button
                  className="view-all-btn"
                  onClick={() => {
                    setFullName(profile?.fullName || "");
                    setIsEditing(true);
                  }}
                >
                  Edit
                </button>
              )}
            </div>
          </div>
          <div className="profile-info">
            <div className="info-item">
              <div className="info-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="info-content">
                <p className="info-label">Full Name</p>
                <EditableField
                  value={fullName}
                  isEditing={isEditing}
                  onChange={setFullName}
                />
              </div>
            </div>
            <div className="info-item">
              <div className="info-icon">
                <i className="bx bx-envelope"></i>
              </div>
              <div className="info-content">
                <p className="info-label">Email</p>
                <p className="info-value">{profile?.email || "N/A"}</p>
              </div>
            </div>
          </div>
        </div>
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Registered Business List</h3>
            <div className="edit-actions">
              <button
                className="view-all-btn"
                onClick={() => setIsModalOpen(true)}
              >
                Add
              </button>
            </div>
          </div>

          <div
            className="table-responsive"
            style={{ maxHeight: "300px", overflowY: "auto" }}
          >
            <table className="table table-bordered table-hover table-striped">
              <thead className="table-light sticky-top">
                <tr>
                  <th scope="col">Sl</th>
                  <th scope="col">Name</th>
                  <th scope="col">Address</th>
                </tr>
              </thead>
              <tbody>
                {shops.length === 0 ? (
                  <tr>
                    <td colSpan={3} className="text-center">
                      No registered shops found.
                    </td>
                  </tr>
                ) : (
                  shops.map((shop, idx) => (
                    <tr key={shop.id || idx}>
                      <td>{idx + 1}</td>
                      <td>{shop.name}</td>
                      <td>{shop.address}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

        <div className="dashboard-card">
          <div className="card-header">
            <h3>Account Settings</h3>
          </div>
          <div className="settings-list">
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-lock-alt"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Change Password</p>
                <span className="settings-description">
                  Last changed 30 days ago
                </span>
              </div>
            </div>
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-bell"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Notification Preferences</p>
                <span className="settings-description">
                  Email notifications enabled
                </span>
              </div>
            </div>
            <div className="settings-item">
              <div className="settings-icon">
                <i className="bx bx-shield"></i>
              </div>
              <div className="settings-content">
                <p className="settings-text">Security Settings</p>
                <span className="settings-description">
                  Two-factor authentication disabled
                </span>
              </div>
            </div>
          </div>
        </div>

        <div className="dashboard-card">
          <div className="card-header">
            <h3>Recent Activity</h3>
            <button className="view-all-btn">View All</button>
          </div>
          <div className="activity-list">
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-lock-alt"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Password changed</p>
                <span className="activity-time">30 days ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-bell"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Notification settings updated</p>
                <span className="activity-time">45 days ago</span>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon">
                <i className="bx bx-user"></i>
              </div>
              <div className="activity-content">
                <p className="activity-text">Profile information updated</p>
                <span className="activity-time">60 days ago</span>
              </div>
            </div>
          </div>
        </div>
      </div>
      <GenericModal
        show={isModalOpen}
        title="Add Shop"
        onClose={() => setIsModalOpen(false)}

      >
        {/* Place your add shop form or content here */}
        <div>
          <p>Add shop form goes here.</p>
        </div>
      </GenericModal>
    </div>
  );
};

export default Profile;

import React, { useEffect, useState } from "react";
import "./Profile.css";
import {
  createOrUpdateShop,
  deleteShop,
  getShopById,
  getUserProfile,
  updateUserProfile,
} from "../api/apiService";
import {  getRoleIdFromCookie, getUserIdFromToken } from "../utils/cookieUtils";
import { CreateOrUpdateShopDto, ProfileType, RegisteredShops } from "../types";
import EditableField from "../components/editable-input-field/EditableField";
import GenericModal from "../components/modal/Modal";
import CustomFormComponent, {
  FormInputConfig,
} from "../components/customFormComponent/CustomFormComponent";

const Profile: React.FC = () => {
  const [profile, setProfile] = useState<ProfileType | null>(null);
  const [shops, setShops] = useState<RegisteredShops[]>([]);
  const [isEditingProfile, setIsEditingProfile] = useState(false);
  const [isEditModeShop, setIsEditModeShop] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const [fullName, setFullName] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [shopForm, setShopForm] = useState<CreateOrUpdateShopDto>({ id: 0, name: "", address: "" });
  const [shopToDelete, setShopToDelete] = useState<number | null>(null);
  const roleId = getRoleIdFromCookie();
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
  const handleSaveProfile = async () => {
    if (!profile) return;
    try {
      await updateUserProfile({
        id: profile.id,
        fullName,
      });
      fetchProfile();
      setIsEditingProfile(false);
    } catch (err) {
      console.error("Update failed", err);
    }
  };
  const handleShopInputChange =
    (field: keyof CreateOrUpdateShopDto) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      setShopForm({ ...shopForm, [field]: e.target.value });
    };
  const handleAddShopOpen = () => {
    setShopForm({ id: 0, name: "", address: "" });
    setIsEditModeShop(false);
    setIsModalOpen(true);
  };
  const handleShopRowClick = async (shopId: number) => {
    try {
      const shop = await getShopById(shopId);
      setShopForm(shop);
      setIsEditModeShop(true);
      setIsModalOpen(true);
    } catch (err) {
      console.error("Failed to fetch shop details", err);
    }
  };
  const handleShopSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await createOrUpdateShop(shopForm);
      fetchProfile();
      setIsModalOpen(false);
      setShopForm({ id: 0, name: "", address: "" });
      setIsEditModeShop(false);
    } catch (err) {
      console.error("Failed to save shop", err);
    }
  };
  const handleDeleteClick = (shopId: number) => {
    setShopToDelete(shopId);
    setIsDeleteModalOpen(true);
  };
  const handleConfirmDelete = async () => {
    if (shopToDelete !== null) {
      try {
        await deleteShop(shopToDelete);
        fetchProfile();
      } catch (err) {
        console.error("Failed to delete shop", err);
      } finally {
        setIsDeleteModalOpen(false);
        setShopToDelete(null);
      }
    }
  };

  const shopInputs: FormInputConfig[] = [
    {
      id: "shopName",
      label: "Shop Name",
      type: "text",
      value: shopForm.name,
      required: true,
      placeholder: "Enter shop name",
      onChange: handleShopInputChange("name"),
    },
    {
      id: "shopAddress",
      label: "Address",
      type: "text",
      value: shopForm.address,
      required: true,
      placeholder: "Enter address",
      onChange: handleShopInputChange("address"),
    },
  ];
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
              {isEditingProfile ? (
                <>
                  <button className="view-all-btn" onClick={handleSaveProfile}>
                    Save
                  </button>
                  <button
                    className="cancel-btn"
                    onClick={() => setIsEditingProfile(false)}
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
                    setIsEditingProfile(true);
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
                  isEditing={isEditingProfile}
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
        {(roleId === 0 || roleId === 1) && (
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Registered Business List</h3>
            <div className="edit-actions">
              <button
                className="view-all-btn"
                onClick={handleAddShopOpen}
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
                  <th scope="col">Action</th>
                </tr>
              </thead>
              <tbody>
                {shops.length === 0 ? (
                  <tr>
                    <td colSpan={4} className="text-center">
                      No registered shops found.
                    </td>
                  </tr>
                ) : (
                  shops.map((shop, idx) => (
                    <tr key={shop.id || idx}>
                      <td onClick={() => handleShopRowClick(shop.id)} style={{ cursor: "pointer" }}>{idx + 1}</td>
                      <td onClick={() => handleShopRowClick(shop.id)} style={{ cursor: "pointer" }}>{shop.name}</td>
                      <td onClick={() => handleShopRowClick(shop.id)} style={{ cursor: "pointer" }}>{shop.address}</td>
                      <td>
                        <span
                          style={{ color: "#dc3545", cursor: "pointer", fontSize: 18 }}
                          title="Delete"
                          onClick={(e) => {
                            e.stopPropagation();
                            handleDeleteClick(shop.id);
                          }}
                        >
                          <i className="bx bx-trash"></i>
                        </span>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
        )}
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
        title={isEditModeShop ? "Edit Shop" : "Add Shop"}
        onClose={() => setIsModalOpen(false)}
      >
        <CustomFormComponent
          inputs={shopInputs}
          onSubmit={handleShopSubmit}
          onCancel={() => setIsModalOpen(false)}
          submitLabel={isEditModeShop ? "Update" : "Save"}
          cancelLabel="Cancel"
        />
      </GenericModal>
      <GenericModal
        show={isDeleteModalOpen}
        title="Delete Shop"
        onClose={() => setIsDeleteModalOpen(false)}
      >
        <div>
          <p>Are you sure you want to delete this shop?</p>
          <div className="d-flex justify-content-end">
            <button className="btn btn-secondary me-2" onClick={() => setIsDeleteModalOpen(false)}>
              Cancel
            </button>
            <button className="btn btn-danger" onClick={handleConfirmDelete}>
              Delete
            </button>
          </div>
        </div>
      </GenericModal>
    </div>
  );
};

export default Profile;

import React, { useEffect, useRef, useState } from "react";
import "./UserManagement.css";
import {
  CreateUserRequest,
  KeyValuePairDto,
  PermissionDetails,
  ProfileType,
} from "../types";
import {
  createUser,
  deleteUser,
  getAllUserOfShop,
  getAvailableRoles,
  getPermissionsOfUser,
  updatePermissionsOfUser,
} from "../api/apiService";
import GenericModal from "../components/modal/Modal";
import CustomFormComponent, {
  FormInputConfig,
} from "../components/customFormComponent/CustomFormComponent";

const UserManagement: React.FC = () => {
  const [users, setUsers] = useState<ProfileType[]>([]);
  const [permissions, setPermissions] = useState<PermissionDetails[]>([]);
  const [roles, setRoles] = useState<KeyValuePairDto[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form, setForm] = useState<CreateUserRequest>({
    fullName: "",
    email: "",
    roleId: 0,
    shopId: 0,
  });
  const [isPermissionModalOpen, setIsPermissionModalOpen] = useState(false);
  const [editablePermissions, setEditablePermissions] = useState<
    PermissionDetails[]
  >([]);
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const PERMISSION_FIELDS = [
    "isCreate",
    "isView",
    "isEdit",
    "isList",
    "isDelete",
  ];
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [userToDelete, setUserToDelete] = useState<string | null>(null);

  const masterCheckboxRef = useRef<HTMLInputElement>(null);
  const rowCheckboxRefs = useRef<(HTMLInputElement | null)[]>([]);

  // Fetch users only once on mount
  useEffect(() => {
    fetchUsers();
    // eslint-disable-next-line
  }, []);

  // Sync editablePermissions when permissions change
  useEffect(() => {
    setEditablePermissions(JSON.parse(JSON.stringify(permissions)));
  }, [permissions]);

  // Indeterminate logic for checkboxes
  useEffect(() => {
    // Master checkbox logic
    const allChecked =
      editablePermissions.length > 0 &&
      editablePermissions.every((perm) =>
        PERMISSION_FIELDS.every(
          (field) => perm[field as keyof PermissionDetails]
        )
      );
    const noneChecked =
      editablePermissions.length > 0 &&
      editablePermissions.every((perm) =>
        PERMISSION_FIELDS.every(
          (field) => !perm[field as keyof PermissionDetails]
        )
      );
    if (masterCheckboxRef.current) {
      masterCheckboxRef.current.indeterminate = !allChecked && !noneChecked;
      masterCheckboxRef.current.checked = allChecked;
    }

    // Row-level checkboxes
    editablePermissions.forEach((perm, idx) => {
      const all = PERMISSION_FIELDS.every(
        (field) => perm[field as keyof PermissionDetails]
      );
      const none = PERMISSION_FIELDS.every(
        (field) => !perm[field as keyof PermissionDetails]
      );
      const checkbox = rowCheckboxRefs.current[idx];
      if (checkbox) {
        checkbox.indeterminate = !all && !none;
        checkbox.checked = all;
      }
    });
  }, [editablePermissions,PERMISSION_FIELDS]);
  const fetchUsers = async () => {
    try {
      const data = await getAllUserOfShop();
      if (Array.isArray(data)) {
        setUsers(data);
      } else {
        setUsers([]);
      }
    } catch (error) {
      setUsers([]);
    }
  };

  const fetchAvailableRoles = async () => {
    try {
      const data = await getAvailableRoles();
      setRoles(data);
    } catch (error) {
      console.error("Error fetching roles:", error);
    }
  };

  const fetchPermissionsOfUser = async (userId: string) => {
    try {
      const data = await getPermissionsOfUser(userId);
      setPermissions(data);
    } catch (error) {
      console.error("Error fetching user permissions:", error);
    }
  };

  const handleInputChange =
    (field: keyof CreateUserRequest) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      setForm({
        ...form,
        [field]: field === "roleId" ? Number(e.target.value) : e.target.value,
      });
    };

  const handleOpenModal = () => {
    fetchAvailableRoles();
    setForm({
      fullName: "",
      email: "",
      roleId: 0,
      shopId: 0,
    });
    setIsModalOpen(true);
  };

  const handleSaveUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await createUser({ ...form });
      setIsModalOpen(false);
      fetchUsers();
    } catch (error) {
      console.error("Error creating user:", error);
    }
  };

  const handleEditPermissions = async (userId: string) => {
    setSelectedUserId(userId);
    await fetchPermissionsOfUser(userId);
    setIsPermissionModalOpen(true);
  };

  const handlePermissionCheckboxChange = (
    permIdx: number,
    field: keyof PermissionDetails
  ) => {
    setEditablePermissions((prev) =>
      prev.map((perm, idx) =>
        idx === permIdx ? { ...perm, [field]: !perm[field] } : perm
      )
    );
  };

  const userInputs: FormInputConfig[] = [
    {
      id: "fullName",
      label: "Full Name",
      type: "text",
      value: form.fullName,
      required: true,
      placeholder: "Enter full name",
      onChange: handleInputChange("fullName"),
    },
    {
      id: "email",
      label: "Email",
      type: "email",
      value: form.email,
      required: true,
      placeholder: "Enter email",
      onChange: handleInputChange("email"),
    },
    {
      id: "roleId",
      label: "Role",
      type: "select",
      value: String(form.roleId),
      required: true,
      options: roles.map((r) => ({ value: String(r.key), label: r.value })),
      onChange: handleInputChange("roleId"),
    },
  ];

  return (
    <div className="user-mgmt-container">
      <div className="user-mgmt-card">
        <div className="user-mgmt-card-header">
          <h3>User List</h3>
          <button className="user-mgmt-add-btn" onClick={handleOpenModal}>
            + New
          </button>
        </div>
        <div className="user-mgmt-table-responsive">
          <table className="user-mgmt-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {users.map((u) => (
                <tr key={u.id}>
                  <td>{u.fullName}</td>
                  <td>{u.email}</td>
                  <td>{u.roleName}</td>
                  <td>
                    <button
                      className="user-mgmt-edit-btn"
                      title="Edit"
                      onClick={() => handleEditPermissions(u.id)}
                      style={{ marginRight: "8px" }}
                    >
                      <i className="bx bx-edit"></i>
                    </button>
                    |
                    <button
                      className="user-mgmt-delete-btn"
                      title="Delete"
                      onClick={() => {
                        setUserToDelete(u.id);
                        setIsDeleteModalOpen(true);
                      }}
                      style={{ marginLeft: "8px" }}
                    >
                      <i className="bx bx-trash"></i>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <GenericModal
        show={isModalOpen}
        title="Add User"
        onClose={() => setIsModalOpen(false)}
      >
        <CustomFormComponent
          inputs={userInputs}
          onSubmit={handleSaveUser}
          onCancel={() => setIsModalOpen(false)}
          submitLabel="Save"
          cancelLabel="Cancel"
        />
      </GenericModal>

      <GenericModal
        show={isPermissionModalOpen}
        title="User Permissions"
        onClose={() => setIsPermissionModalOpen(false)}
      >
        <div className="permissions-table-wrapper">
          <table className="permissions-table">
            <thead>
              <tr>
                <th>
                  <input
                    type="checkbox"
                    ref={masterCheckboxRef}
                    onChange={(e) => {
                      const checked = e.target.checked;
                      setEditablePermissions((prev) =>
                        prev.map((perm) => {
                          const updated: PermissionDetails = { ...perm };
                          PERMISSION_FIELDS.forEach((field) => {
                            (updated as any)[field] = checked;
                          });
                          return updated;
                        })
                      );
                    }}
                  />
                </th>
                <th>Module</th>
                {PERMISSION_FIELDS.map((field) => (
                  <th key={field}>{field.replace("is", "")}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {editablePermissions.map((perm, permIdx) => (
                <tr key={perm.id}>
                  <td>
                    <input
                      type="checkbox"
                      ref={(el) => {
                        rowCheckboxRefs.current[permIdx] = el;
                      }}
                      onChange={(e) => {
                        const checked = e.target.checked;
                        setEditablePermissions((prev) =>
                          prev.map((p, idx) => {
                            if (idx === permIdx) {
                              const updated = { ...p };
                              PERMISSION_FIELDS.forEach((field) => {
                                (updated as any)[field] = checked;
                              });
                              return updated;
                            }
                            return p;
                          })
                        );
                      }}
                    />
                  </td>
                  <td>{perm.moduleName}</td>
                  {PERMISSION_FIELDS.map((field) => (
                    <td key={field}>
                      <input
                        type="checkbox"
                        checked={
                          perm[field as keyof PermissionDetails] as boolean
                        }
                        onChange={() =>
                          handlePermissionCheckboxChange(
                            permIdx,
                            field as keyof PermissionDetails
                          )
                        }
                      />
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
          <div className="d-flex justify-content-end">
            <button
              className="btn btn-primary mt-3"
              onClick={async () => {
                try {
                  const payload = editablePermissions.map((perm) => ({
                    id: perm.id,
                    isCreate: !!perm.isCreate,
                    isView: !!perm.isView,
                    isEdit: !!perm.isEdit,
                    isList: !!perm.isList,
                    isDelete: !!perm.isDelete,
                  }));
                  await updatePermissionsOfUser(selectedUserId, payload);
                  setIsPermissionModalOpen(false);
                } catch (error) {
                  console.error("Failed to update permissions", error);
                }
              }}
            >
              Save
            </button>
          </div>
        </div>
      </GenericModal>

      <GenericModal
        show={isDeleteModalOpen}
        title="Confirm Delete"
        onClose={() => setIsDeleteModalOpen(false)}
      >
        <div>
          <p>Are you sure you want to delete this user?</p>
          <div className="d-flex justify-content-end">
            <button
              className="btn btn-secondary me-2"
              onClick={() => setIsDeleteModalOpen(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger"
              onClick={async () => {
                if (userToDelete) {
                  try {
                    await deleteUser(userToDelete);
                    setIsDeleteModalOpen(false);
                    setUserToDelete(null);
                    fetchUsers();
                  } catch (error) {
                    // Optionally show error
                    setIsDeleteModalOpen(false);
                    setUserToDelete(null);
                  }
                }
              }}
            >
              Delete
            </button>
          </div>
        </div>
      </GenericModal>
    </div>
  );
};

export default UserManagement;

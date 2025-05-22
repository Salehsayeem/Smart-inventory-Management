import React from "react";

interface EditableFieldProps {
  value: string;
  isEditing: boolean;
  onChange: (newValue: string) => void;
}

const EditableField: React.FC<EditableFieldProps> = ({
  value,
  isEditing,
  onChange,
}) => {
  return (
    <>
      {isEditing ? (
        <div>
          <input
            type="text"
            value={value}
            onChange={(e) => onChange(e.target.value)}
            className="edit-input"
          />
           <span className="focus-border"></span>
        </div>
      ) : (
        <p className="edit-input">{value || "N/A"}</p>
      )}
    </>
  );
};

export default EditableField;

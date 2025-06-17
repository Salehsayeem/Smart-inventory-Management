import React, {Fragment} from "react";

export interface FormInputConfig {
  id: string;
  label: string;
  type: string; // text, email, select, checkbox, radio, etc.
  value: string | boolean;
  required?: boolean;
  placeholder?: string;
  onChange: (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => void;
  options?: { value: string; label: string }[]; // for select/radio
  checked?: boolean; // for checkbox/radio
}

interface CustomFormComponentProps {
  inputs: FormInputConfig[];
  onSubmit: (e: React.FormEvent) => void;
  onCancel: () => void;
  submitLabel?: string;
  cancelLabel?: string;
}

const CustomFormComponent: React.FC<CustomFormComponentProps> = ({
  inputs,
  onSubmit,
  onCancel,
  submitLabel = "Save",
  cancelLabel = "Cancel",
}) => (
  <form onSubmit={onSubmit}>
    {inputs.map((input) => (
      <div className="mb-3" key={input.id} style={{ position: "relative" }}>
        <label htmlFor={input.id} className="form-label">
          {input.label}
        </label>
        {input.type === "select" ? (
          <Fragment>
            <select
              id={input.id}
              className="form-select"
              value={typeof input.value === "boolean" ? (input.value ? "true" : "") : input.value}
              onChange={input.onChange}
              required={input.required}
              style={{
                appearance: "none",
                paddingRight: "2.5rem",
                borderRadius: "0.75rem",
                boxShadow: "0 1px 4px rgba(0,0,0,0.07)",
                border: "1px solid #ced4da",
                transition: "box-shadow 0.2s, border-color 0.2s",
                minHeight: "44px",
                fontSize: "1rem",
                backgroundColor: "#fff",
              }}
              onFocus={e => (e.currentTarget.style.boxShadow = "0 0 0 2px #0d6efd33")}
              onBlur={e => (e.currentTarget.style.boxShadow = "0 1px 4px rgba(0,0,0,0.07)")}
            >
              <option value="">Select {input.label}</option>
              {input.options &&
                input.options.map((opt) => (
                  <option key={opt.value} value={opt.value}>
                    {opt.label}
                  </option>
                ))}
            </select>
            <span
              style={{
                position: "absolute",
                right: "1.25rem",
                top: "60%",
                pointerEvents: "none",
                transform: "translateY(-50%)",
                color: "#0d6efd",
                fontSize: "1.4rem",
                display: "flex",
                alignItems: "center",
                height: "100%",
              }}
            >
              <i className="bx bx-chevron-down" style={{ pointerEvents: "none" }}></i>
            </span>
          </Fragment>
        ) : (
          <input
            id={input.id}
            type={input.type}
            className="form-control"
            value={typeof input.value === "boolean" ? (input.value ? "true" : "") : input.value}
            onChange={input.onChange}
            required={input.required}
            placeholder={input.placeholder}
            style={{ borderRadius: "0.75rem", minHeight: "44px", fontSize: "1rem" }}
          />
        )}
      </div>
    ))}
    <div className="d-flex justify-content-end">
      <button
        type="button"
        className="btn btn-secondary me-2"
        onClick={onCancel}
      >
        {cancelLabel}
      </button>
      <button type="submit" className="btn btn-primary">
        {submitLabel}
      </button>
    </div>
  </form>
);

export default CustomFormComponent;

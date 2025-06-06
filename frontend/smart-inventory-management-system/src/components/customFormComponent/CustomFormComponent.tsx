import React from "react";

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
}) => {
  return (
    <form onSubmit={onSubmit}>
      {inputs.map((input) => {
        const commonProps = {
          id: input.id,
          className: "form-control",
          value: input.value as string,
          onChange: input.onChange,
          required: input.required,
        };

        const renderInput = () => {
          switch (input.type) {
            case "select":
              return (
                <select {...commonProps as React.SelectHTMLAttributes<HTMLSelectElement>}>
                  <option value="">Select {input.label}</option>
                  {input.options?.map((opt) => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
              );

            case "checkbox":
              return (
                <div className="form-check">
                  <input
                    type="checkbox"
                    id={input.id}
                    checked={input.checked as boolean}
                    onChange={input.onChange}
                    className="form-check-input"
                    required={input.required}
                  />
                  <label htmlFor={input.id} className="form-check-label">
                    {input.label}
                  </label>
                </div>
              );

            case "radio":
              return (
                <div>
                  <label className="form-label d-block mb-1">{input.label}</label>
                  {input.options?.map((opt) => (
                    <div className="form-check form-check-inline" key={opt.value}>
                      <input
                        className="form-check-input"
                        type="radio"
                        name={input.id}
                        id={`${input.id}-${opt.value}`}
                        value={opt.value}
                        checked={input.value === opt.value}
                        onChange={input.onChange}
                        required={input.required}
                      />
                      <label className="form-check-label" htmlFor={`${input.id}-${opt.value}`}>
                        {opt.label}
                      </label>
                    </div>
                  ))}
                </div>
              );

            default:
              return (
                <>
                  <input
                    {...commonProps}
                    type={input.type}
                    placeholder={input.placeholder}
                  />
                  <label htmlFor={input.id}>{input.label}</label>
                </>
              );
          }
        };

        return (
          <div className="form-floating mb-3" key={input.id}>
            {renderInput()}
          </div>
        );
      })}

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
};

export default CustomFormComponent;

import React from "react";

export interface FormInputConfig {
  id: string;
  label: string;
  type: string;
  value: string;
  required?: boolean;
  placeholder?: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
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
      <div className="mb-3" key={input.id}>
        <label htmlFor={input.id} className="form-label">
          {input.label}
        </label>
        <input
          id={input.id}
          type={input.type}
          className="form-control"
          value={input.value}
          onChange={input.onChange}
          required={input.required}
          placeholder={input.placeholder}
        />
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
import React from "react";
import { Modal } from "react-bootstrap";
import "./Modal.css";

interface GenericModalProps {
  show: boolean;
  title?: string;
  onClose: () => void;
  children: React.ReactNode;
  footer?: React.ReactNode;
  centered?: boolean;
}

const GenericModal: React.FC<GenericModalProps> = ({
  show,
  title,
  onClose,
  children,
  footer,
  centered = true,
}) => (
  <Modal show={show} onHide={onClose} centered={centered}>
    {title && (
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
    )}
    <Modal.Body>{children}</Modal.Body>
    {footer && <Modal.Footer>{footer}</Modal.Footer>}
  </Modal>
);

export default GenericModal;
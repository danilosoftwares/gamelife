import { useAlertStore } from "../hooks/useAlert";
import { useEffect, useState } from "react";

const FADE_DURATION = 700; // ms

const AlertMessage = () => {
  const { message, type } = useAlertStore();
  const [show, setShow] = useState(false);
  const [shouldRender, setShouldRender] = useState(false);

  useEffect(() => {
    if (message) {
      setShow(true);
      setShouldRender(true);
      const timer = setTimeout(() => setShow(false), 3500);
      return () => clearTimeout(timer);
    }
  }, [message]);

  useEffect(() => {
    if (!show && shouldRender) {
      const timeout = setTimeout(() => setShouldRender(false), FADE_DURATION);
      return () => clearTimeout(timeout);
    }
  }, [show, shouldRender]);

  if (!shouldRender) return null;

  return (
    <div
      style={{
        position: "absolute",
        display: "flex",
        top: "1px",
        right: "1px",
        margin: "50px",
        background: "white",
        padding: "32px",
        color:
          type === "error"
            ? "red"
            : type === "info"
            ? "blue"
            : type === "success"
            ? "green"
            : "black",
        borderRadius: "16px",
        boxShadow: "0 4px 24px rgba(0,0,0,0.12)",
        opacity: show ? 1 : 0,
        transition: `opacity ${FADE_DURATION}ms ease`
      }}
    >
      {message}
    </div>
  );
};

export default AlertMessage;

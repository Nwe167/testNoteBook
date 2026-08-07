export {};

declare global {
  interface Window {
    google?: {
      accounts: {
        id: {
          initialize: (config: {
            client_id: string;

            callback: (
              response: {
                credential: string;
                select_by?: string;
                clientId?: string;
              }
            ) => void;

            ux_mode?: "popup" | "redirect";

            auto_select?: boolean;
          }) => void;

          renderButton: (
            parent: HTMLElement,
            options: {
              theme?: string;
              size?: string;
              text?: string;
              shape?: string;
              width?: number;
            }
          ) => void;

          cancel: () => void;
        };
      };
    };
  }
}
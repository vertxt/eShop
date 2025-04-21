import { ReactNode } from "react";

export type NavigationItem = {
  id: string;
  title: string;
  path?: string;
  icon?: ReactNode;
  items?: NavigationItem[]
}

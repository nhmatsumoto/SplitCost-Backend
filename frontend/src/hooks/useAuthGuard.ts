// import { useEffect, useState } from "react";
// import { useKeycloak } from "@react-keycloak/web";
// import useAuthStore, { UserProfile, UserRoles } from "../store/authStore";

// interface KeycloakTokenParsed {
//   sub?: string;
//   preferred_username?: string;
//   email?: string;
//   realm_access?: {
//     roles?: string[];
//   };
//   resource_access?: {
//     [key: string]: {
//       roles?: string[];
//     };
//   };
// }

// interface AuthGuardResult {
//   isAuthenticated: boolean;
//   initialized: boolean;
//   keycloak: ReturnType<typeof useKeycloak>['keycloak'];
//   user: UserProfile | null;
//   roles: UserRoles | null;
// }

// const useAuthGuard = (): AuthGuardResult => {
//   const { keycloak, initialized } = useKeycloak();
//   const [hasTriedLogin, setHasTriedLogin] = useState(false);
//   const { setAuthInfo } = useAuthStore();

//   useEffect(() => {
//     if (initialized) {
//       if (keycloak.authenticated) {

//         const tokenParsed = keycloak.tokenParsed as KeycloakTokenParsed | undefined;

//         const userProfile: UserProfile = {
//           id: tokenParsed?.sub,
//           username: tokenParsed?.preferred_username,
//           email: tokenParsed?.email,
//           residence: null,
//         };

//         let userRoles: UserRoles = {};
//         const realmRoles = tokenParsed?.realm_access?.roles || [];
//         const clientRoles = tokenParsed?.resource_access?.['split-costs-client']?.roles || [];

//         userRoles.isAdmin = realmRoles.includes('admin') || clientRoles.includes('admin');
//         userRoles.isOwner = realmRoles.includes('owner') || clientRoles.includes('owner');
//         userRoles.isMember = realmRoles.includes('member') || clientRoles.includes('member');

//         setAuthInfo(true, userProfile, userRoles);
//         setHasTriedLogin(true);

//       } else if (!hasTriedLogin) {
//         keycloak.login();
//         setHasTriedLogin(true);
//       }
//     }
//   }, [initialized, keycloak, setAuthInfo, hasTriedLogin]);

//   return {
//     isAuthenticated: useAuthStore((state) => state.isAuthenticated),
//     initialized,
//     keycloak,
//     user: useAuthStore((state) => state.userProfile),
//     roles: useAuthStore((state) => state.userRoles),
//   };
// };

// export default useAuthGuard;
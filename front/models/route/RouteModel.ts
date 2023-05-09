export default interface RouteModel {
    id: string,
    matchPath?: string,
    matchHosts: string[],
    clusterId: string,
    enabled: boolean
}
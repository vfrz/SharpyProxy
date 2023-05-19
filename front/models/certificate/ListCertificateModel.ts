import CertificateType from "~/models/certificate/CertificateType";

export default interface ListCertificateModel {
    id: string,
    name: string,
    expiration: string,
    domain: string,
    type: CertificateType
}
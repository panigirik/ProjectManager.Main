// dropboxClient.js
import axios from 'axios';

const DROPBOX_ACCESS_TOKEN = 'sl.u.AFzoNLc8SBkfr56xCflN_nVw38eia9ttiiZjY6EFcizRhIjhRhsTSD5hU1_QTImCpYXD0AcSXBGWn9MPyWSdtvQGZ0yj2FgpRPKPhz6pNdzvBouwK68LYTsgm0yS-Pe-Y6HOf390yJDu1DtXCF4ZDz6rVy8IT2Xt2bpPHNBrl2ASvGEbAG6kCXZOWKhVgXUr8DHZ-B_Fm9ARnGBnl2GCqThtn21VgT9ygDa4pyU4Ri2Fxwy0baFUtujxY5vgDKnCCxQDk4aiNz1_iGrIYpE_hBocQ0h-IhLf4XwLzeLD05e-ZlyrWIg71P7uK-7zg6ocErtDZ07QGi_fHLQ9O9nrQNBofakKyF5bt8L8c1IByrOI9yZO0UyCX4F_phhMzt2T-RDDbJIkYEv6TclnDeCOOs9C0UAPQgidxry05I95Y9u78DRr5280aI06BX9O7iV_sU83EIFSlTzsLqQD2XBr6xuXPUXMX_XlIpLTcaVsQGLtDpbTd-tdTRI2BTcQKyzNgamdS5qFq1kEREGuXgIaowF7Zdio3pBKH_0QGhxxlfeBbfZJu8QLepxFlPe-X_klenEtQcr-5kJDFeibXW5XivHh2nS2lfBIvmUuW2xTAjNr9yD8midgBBPwlt4MXxfOAQ-lL1uWTU4dBYHaaUHSryEF1rf54uWW2WnhoOoQqS3KPP3qrOXHrZR558wkD7UG19_zCFPgfpufteacoWY62Q3cLJEyahZuQyl7xXDMU9gXynSmT_xihLPYkGEH_LeDyvunLZQVo8xQo3VKbjnNw0kOcnXtkDhgntbRt-WECL6aKvnrCNp0Q-ElKUnw6if6tirll_-4xZbJ-bQlGzZFf-yYuHjnqZmUmIVqdeviPzHkPykJaeFzgiX_eSi3IoHsOESZhiAkq9ki3XXN2qynHAlPRPqSViUkdemzix7nIZtJusQj_vBsztp6V_S2KDHmWCGiAlJ8wzGrT6il9QSDJJyo4g7626KreIM52cdKAsR2ygeLIBpk5ir0Z4ZHXBVwhDLhHu3136-vmQzySB9sObQOJAXxn5fP10wGRUPtNB8HkVEFhexKEgfJoBajsqs2wqjMtFI60RlCELP7MkeXuOHuU8uw74_q6vA-foMxuo6qqfi9jfurLpdtKjsu0jqRSW45ZQ336-ybaRbIy3SSFKypFYE4ueBYP6o0p36XbuZXq2VpJiVkmjqzjiG0StxPM_LLy65onvL86YGYYBvbO3eteM25hZIM92RyFaqQJlRzjlhDNhvcG6upcKjosc4tHYj90YARRYRsZGTOENTfSqx8VWNOURg_M23hzRV9PNzcwhbeKCX4aTpzxWV3gw6-Em225fUtCGN5omXCLM1iITDKoYqLq2FZ1V1cfbLA5RhyC9JPM1q3stQCtOiotQriTj5yrMp9__QnNfsk8N8Q2ZGAacHFWyb7QLSBjMGTzWBtPw'; // подставь сюда свой
export async function getDropboxTemporaryLink(path) {
    try {
      const response = await axios.post(
        'https://api.dropboxapi.com/2/files/get_temporary_link',
        { path },
        {
          headers: {
            Authorization: `Bearer ${DROPBOX_ACCESS_TOKEN}`,
            'Content-Type': 'application/json',
          },
        }
      );
      console.log(`Dropbox temp link for ${path}:`, response.data.link);
      return response.data.link;
    } catch (error) {
      console.error('Ошибка при получении временной ссылки:', error.response?.data || error.message);
      return null;
    }
  }
  

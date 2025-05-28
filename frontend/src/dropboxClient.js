// dropboxClient.js
import axios from 'axios';

const DROPBOX_ACCESS_TOKEN = 'https://uc349344dbbfbe422f8d65c750b4.dl.dropboxusercontent.com/cd/0/get/Cqh5vnm_h0tXmw2E7uFRR8bqpdEdZgor9mTJlWE0S-z9f7rnPXni1cmSy3QrSzAC6tmxW1D-E7c6xve5oLGvsQ2eO1ylZt4NAZw-zPERoNOa8GVJ-mVdnSq8HW13CFvnMSY8WnIFR6ZVB93Eea-cZU2pilu5sEuyeFMEABXK4pOiDA/file'; // подставь сюда свой
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
  
